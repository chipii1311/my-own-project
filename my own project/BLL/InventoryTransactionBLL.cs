using my_own_project.DAL;
using my_own_project.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace my_own_project.BLL
{
    public class InventoryTransactionBLL
    {
        // ==================== VALIDATE ====================

        /// <summary>
        /// Validate dữ liệu giao dịch kho
        /// </summary>
        private static bool ValidateTransaction(InventoryTransactionDTO transaction, out string errorMessage)
        {
            errorMessage = "";

            // Kiểm tra IngredientID
            if (transaction.IngredientID <= 0)
            {
                errorMessage = "Vui lòng chọn nguyên liệu!";
                return false;
            }

            // Kiểm tra Quantity
            if (transaction.QuantityChanged <= 0)
            {
                errorMessage = "Số lượng phải lớn hơn 0!";
                return false;
            }

            // Kiểm tra TransactionType
            if (string.IsNullOrEmpty(transaction.TransactionType))
            {
                errorMessage = "Vui lòng chọn loại giao dịch (Import/Export/Adjustment)!";
                return false;
            }

            // Kiểm tra TransactionType hợp lệ
            if (!IsValidTransactionType(transaction.TransactionType))
            {
                errorMessage = "Loại giao dịch không hợp lệ! (Import/Export/Adjustment)";
                return false;
            }

            // Kiểm tra StaffID
            if (transaction.StaffID <= 0)
            {
                errorMessage = "Vui lòng chọn nhân viên thực hiện!";
                return false;
            }

            // Kiểm tra nếu Export thì phải có đủ tồn kho
            if (transaction.TransactionType == "Export")
            {
                IngredientDTO ingredient = IngredientDAL.GetByID(transaction.IngredientID);
                if (ingredient == null)
                {
                    errorMessage = "Nguyên liệu không tồn tại!";
                    return false;
                }

                if (ingredient.StockQuantity < transaction.QuantityChanged)
                {
                    errorMessage = $"Tồn kho không đủ! Hiện có: {ingredient.StockQuantity}, yêu cầu: {transaction.QuantityChanged}";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra loại giao dịch có hợp lệ không
        /// </summary>
        private static bool IsValidTransactionType(string transactionType)
        {
            return transactionType == "Import" ||
                   transactionType == "Export" ||
                   transactionType == "Adjustment";
        }

        // ==================== CREATE ====================

        /// <summary>
        /// Tạo giao dịch nhập kho
        /// </summary>
        public static int ImportIngredient(int ingredientID, float quantity, int staffID, string note = "")
        {
            try
            {
                InventoryTransactionDTO transaction = new InventoryTransactionDTO
                {
                    IngredientID = ingredientID,
                    QuantityChanged = quantity,
                    TransactionType = "Import",
                    StaffID = staffID,
                    Note = note,
                    TransactionDate = DateTime.Now
                };

                if (!ValidateTransaction(transaction, out string errorMsg))
                    throw new Exception(errorMsg);

                int transactionID = InventoryTransactionDAL.Insert(transaction);

                // Log
                Console.WriteLine($"✅ Nhập kho thành công - ID: {transactionID}, Nguyên liệu: {ingredientID}, Số lượng: {quantity}");

                return transactionID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.ImportIngredient Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tạo giao dịch xuất kho (sử dụng cho nấu ăn)
        /// </summary>
        public static int ExportIngredient(int ingredientID, float quantity, int staffID, string note = "")
        {
            try
            {
                InventoryTransactionDTO transaction = new InventoryTransactionDTO
                {
                    IngredientID = ingredientID,
                    QuantityChanged = quantity,
                    TransactionType = "Export",
                    StaffID = staffID,
                    Note = note,
                    TransactionDate = DateTime.Now
                };

                if (!ValidateTransaction(transaction, out string errorMsg))
                    throw new Exception(errorMsg);

                int transactionID = InventoryTransactionDAL.Insert(transaction);

                // Log
                Console.WriteLine($"✅ Xuất kho thành công - ID: {transactionID}, Nguyên liệu: {ingredientID}, Số lượng: {quantity}");

                return transactionID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.ExportIngredient Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tạo giao dịch điều chỉnh kho (cân bằng kho)
        /// </summary>
        public static int AdjustStock(int ingredientID, float newQuantity, int staffID, string note = "")
        {
            try
            {
                // Lấy tồn kho hiện tại
                IngredientDTO ingredient = IngredientDAL.GetByID(ingredientID);
                if (ingredient == null)
                    throw new Exception("Nguyên liệu không tồn tại!");

                // Tính số lượng thay đổi
                float quantityChanged = newQuantity - ingredient.StockQuantity;

                InventoryTransactionDTO transaction = new InventoryTransactionDTO
                {
                    IngredientID = ingredientID,
                    QuantityChanged = Math.Abs(quantityChanged), // Lưu giá trị tuyệt đối
                    TransactionType = "Adjustment",
                    StaffID = staffID,
                    Note = $"{note} (Từ {ingredient.StockQuantity} → {newQuantity})",
                    TransactionDate = DateTime.Now
                };

                if (!ValidateTransaction(transaction, out string errorMsg))
                    throw new Exception(errorMsg);

                int transactionID = InventoryTransactionDAL.Insert(transaction);

                // Log
                Console.WriteLine($"✅ Điều chỉnh kho thành công - ID: {transactionID}, Nguyên liệu: {ingredientID}");

                return transactionID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.AdjustStock Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================

        /// <summary>
        /// Lấy tất cả giao dịch kho
        /// </summary>
        public static DataTable GetAll()
        {
            try
            {
                return InventoryTransactionDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy giao dịch kho theo nguyên liệu
        /// </summary>
        public static DataTable GetByIngredient(int ingredientID)
        {
            try
            {
                if (ingredientID <= 0)
                    throw new Exception("ID nguyên liệu không hợp lệ!");

                return InventoryTransactionDAL.GetByIngredient(ingredientID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetByIngredient Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy giao dịch kho theo khoảng thời gian
        /// </summary>
        public static DataTable GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Validate
                if (startDate > endDate)
                    throw new Exception("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");

                // Nếu chỉ có ngày, set endDate là cuối ngày
                endDate = endDate.AddDays(1).AddSeconds(-1);

                return InventoryTransactionDAL.GetByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetByDateRange Error: {ex.Message}");
                throw;
            }
        }

        // ==================== REPORT & ANALYSIS ====================

        /// <summary>
        /// Lấy báo cáo tồn kho (Inventory Report)
        /// </summary>
        public static DataTable GetInventoryReport()
        {
            try
            {
                DataTable dtIngredients = IngredientDAL.GetAll();
                DataTable dtReport = new DataTable();

                // Tạo các cột
                dtReport.Columns.Add("IngredientID", typeof(int));
                dtReport.Columns.Add("IngredientName", typeof(string));
                dtReport.Columns.Add("Unit", typeof(string));
                dtReport.Columns.Add("StockQuantity", typeof(float));
                dtReport.Columns.Add("MinStock", typeof(float));
                dtReport.Columns.Add("Status", typeof(string)); // Normal, LowStock, OutOfStock
                dtReport.Columns.Add("Note", typeof(string));

                // Điền dữ liệu
                foreach (DataRow row in dtIngredients.Rows)
                {
                    int ingredientID = (int)row["IngredientID"];
                    float stockQuantity = row["StockQuantity"] != DBNull.Value ? (float)row["StockQuantity"] : 0;
                    float minStock = row["MinStock"] != DBNull.Value ? (float)row["MinStock"] : 0;

                    string status = GetStockStatus(stockQuantity, minStock);
                    string note = GetStockNote(stockQuantity, minStock);

                    dtReport.Rows.Add(
                        ingredientID,
                        row["IngredientName"].ToString(),
                        row["Unit"].ToString(),
                        stockQuantity,
                        minStock,
                        status,
                        note
                    );
                }

                return dtReport;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetInventoryReport Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách nguyên liệu sắp hết (Low Stock)
        /// </summary>
        public static DataTable GetLowStockItems()
        {
            try
            {
                return IngredientDAL.GetLowStock();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetLowStockItems Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách nguyên liệu hết (Out of Stock)
        /// </summary>
        public static DataTable GetOutOfStockItems()
        {
            try
            {
                DataTable dtIngredients = IngredientDAL.GetAll();
                DataTable dtOutOfStock = dtIngredients.Clone();

                foreach (DataRow row in dtIngredients.Rows)
                {
                    float stockQuantity = row["StockQuantity"] != DBNull.Value ? (float)row["StockQuantity"] : 0;
                    if (stockQuantity <= 0)
                        dtOutOfStock.ImportRow(row);
                }

                return dtOutOfStock;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetOutOfStockItems Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy báo cáo nhập/xuất theo loại (Import/Export/Adjustment)
        /// </summary>
        public static DataTable GetTransactionSummaryByType(DateTime startDate, DateTime endDate)
        {
            try
            {
                DataTable dtTransactions = GetByDateRange(startDate, endDate);
                DataTable dtSummary = new DataTable();

                dtSummary.Columns.Add("TransactionType", typeof(string));
                dtSummary.Columns.Add("Count", typeof(int));
                dtSummary.Columns.Add("TotalQuantity", typeof(double));

                // Nhóm theo loại giao dịch
                var summary = dtTransactions.AsEnumerable()
                    .GroupBy(x => x["TransactionType"].ToString())
                    .Select(g => new
                    {
                        Type = g.Key,
                        Count = g.Count(),
                        TotalQty = g.Sum(x => Convert.ToDouble(x["QuantityChanged"]))
                    });

                foreach (var item in summary)
                {
                    dtSummary.Rows.Add(item.Type, item.Count, item.TotalQty);
                }

                return dtSummary;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetTransactionSummaryByType Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tính giá trị tồn kho (Stock Value)
        /// </summary>
        public static decimal CalculateStockValue()
        {
            try
            {
                DataTable dtMenuItems = MenuItemDAL.GetAll();
                decimal totalValue = 0;

                foreach (DataRow row in dtMenuItems.Rows)
                {
                    if (row["Price"] != DBNull.Value && row["StockQuantity"] != DBNull.Value)
                    {
                        decimal price = (decimal)row["Price"];
                        float quantity = (float)row["StockQuantity"];
                        totalValue += price * (decimal)quantity;
                    }
                }

                return totalValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.CalculateStockValue Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER METHODS ====================

        /// <summary>
        /// Lấy trạng thái tồn kho
        /// </summary>
        private static string GetStockStatus(float stockQuantity, float minStock)
        {
            if (stockQuantity <= 0)
                return "OutOfStock";
            else if (stockQuantity < minStock)
                return "LowStock";
            else
                return "Normal";
        }

        /// <summary>
        /// Lấy ghi chú tồn kho
        /// </summary>
        private static string GetStockNote(float stockQuantity, float minStock)
        {
            if (stockQuantity <= 0)
                return "⚠️ Hết hàng - cần nhập gấp!";
            else if (stockQuantity < minStock)
                return "⚠️ Sắp hết - nên nhập thêm";
            else
                return "✅ Bình thường";
        }

        /// <summary>
        /// Lấy hướng dẫn nhập kho dựa trên Min Stock
        /// </summary>
        public static string GetRestockRecommendation(int ingredientID)
        {
            try
            {
                IngredientDTO ingredient = IngredientDAL.GetByID(ingredientID);
                if (ingredient == null)
                    return "Nguyên liệu không tồn tại!";

                float currentStock = ingredient.StockQuantity;
                float minStock = ingredient.MinStock;
                float recommendedQuantity = minStock * 2; // Nhập để có 2 lần Min Stock

                if (currentStock >= minStock)
                    return $"✅ Tồn kho đủ. Hiện có: {currentStock}, Tối thiểu: {minStock}";
                else
                    return $"⚠��� Cần nhập {recommendedQuantity - currentStock} {ingredient.Unit} để đạt {recommendedQuantity}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetRestockRecommendation Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Kiểm tra có thể tạo Order không (đủ nguyên liệu)
        /// </summary>
        public static bool CanCreateOrder(int menuItemID, int quantity)
        {
            try
            {
                DataTable dtRecipe = RecipeDAL.GetByMenuItem(menuItemID);

                foreach (DataRow row in dtRecipe.Rows)
                {
                    int ingredientID = (int)row["IngredientID"];
                    float requiredQuantity = (float)row["Quantity"] * quantity;

                    IngredientDTO ingredient = IngredientDAL.GetByID(ingredientID);
                    if (ingredient == null || ingredient.StockQuantity < requiredQuantity)
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.CanCreateOrder Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách nguyên liệu còn thiếu cho một Order
        /// </summary>
        public static List<string> GetMissingIngredients(int menuItemID, int quantity)
        {
            List<string> missingList = new List<string>();

            try
            {
                DataTable dtRecipe = RecipeDAL.GetByMenuItem(menuItemID);

                foreach (DataRow row in dtRecipe.Rows)
                {
                    int ingredientID = (int)row["IngredientID"];
                    float requiredQuantity = (float)row["Quantity"] * quantity;
                    string ingredientName = row["IngredientName"].ToString();
                    string unit = row["Unit"].ToString();

                    IngredientDTO ingredient = IngredientDAL.GetByID(ingredientID);
                    if (ingredient != null && ingredient.StockQuantity < requiredQuantity)
                    {
                        float shortage = requiredQuantity - ingredient.StockQuantity;
                        missingList.Add($"❌ {ingredientName}: thiếu {shortage} {unit}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionBLL.GetMissingIngredients Error: {ex.Message}");
                throw;
            }

            return missingList;
        }
    }
}
