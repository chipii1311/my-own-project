using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using my_own_project.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace my_own_project.DAL
{
    public class IngredientDAL
    {
        public static DataTable GetAll()
        {
            return DataHelper.ExecuteSPGetTable("sp_Ingredient_GetAll", null);
        }

        public static DataTable GetByID(int id)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IngredientID", id)
            };

            return DataHelper.ExecuteSPGetTable("sp_Ingredient_GetByID", parameters);
        }

        public static int Insert(IngredientDTO ingredient)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IngredientName", ingredient.IngredientName),
                new SqlParameter("@Unit", ingredient.Unit),
                new SqlParameter("@StockQuantity", ingredient.StockQuantity),
                new SqlParameter("@MinStock", ingredient.MinStock),
                new SqlParameter("@PurchasePrice", ingredient.PurchasePrice)
            };

            return DataHelper.ExecuteSP("sp_Ingredient_Insert", parameters);
        }

        public static int Update(IngredientDTO ingredient)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IngredientID", ingredient.IngredientID),
                new SqlParameter("@IngredientName", ingredient.IngredientName),
                new SqlParameter("@Unit", ingredient.Unit),
                new SqlParameter("@StockQuantity", ingredient.StockQuantity),
                new SqlParameter("@MinStock", ingredient.MinStock),
                new SqlParameter("@PurchasePrice", ingredient.PurchasePrice)
            };

            return DataHelper.ExecuteSP("sp_Ingredient_Update", parameters);
        }

        public static int SoftDelete(int id)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IngredientID", id)
            };

            return DataHelper.ExecuteSP("sp_Ingredient_SoftDelete", parameters);
        }

        public static DataTable GetLowStock()
        {
            return DataHelper.ExecuteSPGetTable("sp_Ingredient_GetLowStock", null);
        }

        public static int GetLowStockCount()
        {
            DataTable dt = DataHelper.ExecuteSPGetTable("sp_Ingredient_GetLowStockCount", null);

            if (dt == null || dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["LowStockCount"]);
        }
    }
}
