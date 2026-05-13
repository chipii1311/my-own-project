using my_own_project.DAL;
using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.BLL
{
    public class IngredientBLL
    {
        public static DataTable GetAllIngredients()
        {
            return IngredientDAL.GetAll();
        }

        public static IngredientDTO GetIngredientByID(int id)
        {
            DataTable dt = IngredientDAL.GetByID(id);

            if (dt == null || dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new IngredientDTO
            {
                IngredientID = Convert.ToInt32(row["IngredientID"]),
                IngredientName = row["IngredientName"].ToString(),
                Unit = row["Unit"].ToString(),
                StockQuantity = Convert.ToSingle(row["StockQuantity"]),
                MinStock = Convert.ToSingle(row["MinStock"]),
                PurchasePrice = Convert.ToDecimal(row["PurchasePrice"]),
                IsActive = Convert.ToBoolean(row["IsActive"])
            };
        }

        public static void AddIngredient(IngredientDTO ingredient)
        {
            ValidateIngredient(ingredient, false);
            IngredientDAL.Insert(ingredient);
        }

        public static void UpdateIngredient(IngredientDTO ingredient)
        {
            ValidateIngredient(ingredient, true);
            IngredientDAL.Update(ingredient);
        }

        public static void DeleteIngredient(int id)
        {
            if (id <= 0)
                throw new Exception("Nguyên liệu không hợp lệ.");

            IngredientDAL.SoftDelete(id);
        }

        public static DataTable GetLowStockIngredients()
        {
            return IngredientDAL.GetLowStock();
        }

        public static int GetLowStockCount()
        {
            return IngredientDAL.GetLowStockCount();
        }

        private static void ValidateIngredient(IngredientDTO ingredient, bool isUpdate)
        {
            if (ingredient == null)
                throw new Exception("Dữ liệu nguyên liệu không hợp lệ.");

            if (isUpdate && ingredient.IngredientID <= 0)
                throw new Exception("ID nguyên liệu không hợp lệ.");

            if (string.IsNullOrWhiteSpace(ingredient.IngredientName))
                throw new Exception("Tên nguyên liệu không được để trống.");

            if (string.IsNullOrWhiteSpace(ingredient.Unit))
                throw new Exception("Đơn vị tính không được để trống.");

            if (ingredient.StockQuantity < 0)
                throw new Exception("Số lượng tồn không được âm.");

            if (ingredient.MinStock < 0)
                throw new Exception("Mức tồn tối thiểu không được âm.");

            if (ingredient.PurchasePrice < 0)
                throw new Exception("Giá nhập không được âm.");
        }
    }
}
