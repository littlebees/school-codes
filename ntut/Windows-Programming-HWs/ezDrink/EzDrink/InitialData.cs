using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    public class InitialData
    {
        public const int INDEX_FOR_TITLE = 1;
        public const int INDEX_FOR_PRICE = 2;
        public const int INGREDIENT_INDEX = 2;
        public const int ORDER_INDEX = 3;
        public const int INGREDIENT_BACK_INDEX = 4;
        public const int EMPTY_INDEX = -1;
        public const int DELETE_COLUMN_INDEX = 5;

        // 產生配料初始資料
        public static List<IngredientWhichCanBeAdded> GenerateIngredientWhichCanBeAddedList()
        {
            const int PRICE_OF_PEARL = 5;
            const int PRICE_OF_GO = 5;
            const int PRICE_OF_WEED = 10;
            const int PRICE_OF_PUDDING = 10;
            const string PEARL_TITLE = "珍珠";
            const string GO_TITLE = "椰果";
            const string WEED_TITLE = "仙草";
            const string PUDDING_TITLE = "布丁";

            IngredientWhichCanBeAdded[] ingredients = new IngredientWhichCanBeAdded[] { new IngredientWhichCanBeAdded(new Ingredient(PRICE_OF_PEARL, PEARL_TITLE)), new IngredientWhichCanBeAdded(new Ingredient(PRICE_OF_GO, GO_TITLE)), new IngredientWhichCanBeAdded(new Ingredient(PRICE_OF_WEED, WEED_TITLE)), new IngredientWhichCanBeAdded(new Ingredient(PRICE_OF_PUDDING, PUDDING_TITLE)) };
            return ingredients.ToList();
        }

        // 產生飲料初始資料
        public static List<Drink> GenerateDrinkList()
        {
            const int PRICE_OF_GREEN_TEA = 20;
            const int PRICE_OF_BLACK_TEA = 25;
            const int PRICE_OF_MOUNTAIN_TEA = 40;
            const int PRICE_OF_TIEGUANYIN = 50;
            const int PRICE_OF_OOLONG_TEA = 30;
            const string GREEN_TEA_TITLE = "茉莉綠茶";
            const string BLACK_TEA_TITLE = "阿薩姆紅茶";
            const string MOUNTAIN_TEA_TITLE = "高山青茶";
            const string TIEGUANYIN_TITLE = "鐵觀音";
            const string OOLONG_TEA_TITLE = "烏龍青茶";
            Drink[] drinks = new Drink[] { new EzDrink.Drink(PRICE_OF_GREEN_TEA, GREEN_TEA_TITLE), new EzDrink.Drink(PRICE_OF_BLACK_TEA, BLACK_TEA_TITLE), new EzDrink.Drink(PRICE_OF_MOUNTAIN_TEA, MOUNTAIN_TEA_TITLE), new EzDrink.Drink(PRICE_OF_TIEGUANYIN, TIEGUANYIN_TITLE), new EzDrink.Drink(PRICE_OF_OOLONG_TEA, OOLONG_TEA_TITLE) };
            return drinks.ToList();
        }

        // 把DGV的adapter建好
        public static void EstablishDataGridView(EzDrinkModel model, params DataGridView[] views)
        {
            model.GetAdapterCollection().AddDrinkAdapter(new DataGridViewAdapter<Drink>(views[0], ModifyMerchandiseCells));
            model.GetAdapterCollection().AddDrinkAdapter(new DataGridViewAdapter<Drink>(views[1], ModifyMerchandiseCells));
            model.GetAdapterCollection().IngredientAdapter = new DataGridViewAdapter<IngredientWhichCanBeAdded>(views[INGREDIENT_INDEX],
                delegate (DataGridViewRow row, IngredientWhichCanBeAdded data)
                {
                    ((DataGridViewDisableButtonCell)(row.Cells[0])).Enabled = !(data.IsAdded);
                    ModifyMerchandiseCells(row, data.Top);
                });
            model.GetAdapterCollection().OrderAdapter = new DataGridViewAdapter<OrderItem>(views[ORDER_INDEX], ModifyOrderCell);
            model.GetAdapterCollection().AddAdapterForGridNeedsIngredient(new DataGridViewAdapter<IngredientWhichCanBeAdded>(views[INGREDIENT_BACK_INDEX],
                delegate (DataGridViewRow row, IngredientWhichCanBeAdded data)
                {
                    ModifyMerchandiseCells(row, data.TheIngredient);
                }));
        }

        // 修改Title與Price的helper
        private static void ModifyMerchandiseCells(DataGridViewRow row, IMerchandise data)
        {
            row.Cells[INDEX_FOR_TITLE].Value = data.Title;
            row.Cells[INDEX_FOR_PRICE].Value = data.Price;
        }

        // 產生About的string
        public static String GetAboutString()
        {
            const String VERSION = "V1.0";
            const String DEVELOPER = "102331020";
            const String SYSTEM_NAME = "EzDrink";
            const String LAST_UPDATED_DAY = "2016-10-18";
            const String BREAK = "\n";
            return "版本:" + VERSION + BREAK + "開發者學號:" + DEVELOPER + BREAK + "系統名稱:" + SYSTEM_NAME + BREAK + "最後更新日期" + LAST_UPDATED_DAY;
        }

        // 幫忙建立其他的adapter
        public static void EstablishAdapter(EzDrinkModel model, Label sum, Button sumApply, params Button[] drinkStateButtons)
        {
            model.GetAdapterCollection().SumAdapter = new LabelAdapter(sum, MakeSumString);
            model.GetAdapterCollection().OkAdapter = new ButtonAdapter(sumApply);
            foreach (var button in drinkStateButtons)
                model.GetAdapterCollection().AddDrinkStateAdapter(new ButtonAdapter(button));
        }

        // 總價字串
        public static String MakeSumString(int sumNumber)
        {
            const String TITLE_OF_SUM_LABEL = "總價:";
            return TITLE_OF_SUM_LABEL + sumNumber;
        }

        // order的cell怎麼修改value的helper
        public static void ModifyOrderCell(DataGridViewRow row, OrderItem order)
        {
            for (int index = 0; index < order.OrderDescription.Count; index++)
                row.Cells[index].Value = order.OrderDescription[index];
        }

        // history的adapter初始化
        public static void MakeHistoryTab(EzDrinkModel model, DataGridView history, DataGridView detail, Label sum)
        {
            model.GetAdapterCollection().OrdersAdapter = new DataGridViewAdapter<Order>(history,
                delegate(DataGridViewRow row, Order order)
                {
                    row.Cells[ 0 ].Value = order.Time.ToLongDateString() + " " + order.Time.ToLongTimeString();
                    row.Cells[ 1 ].Value = order.Price + "";
                });
            model.GetAdapterCollection().DetailAdapter = new DataGridViewAdapter<OrderItem>(detail, ModifyOrderCell);
            model.GetAdapterCollection().HistorySumAdapter = new LabelAdapter(sum, MakeSumString);
        }
    }
}
