using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzDrink;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace EzDrink.Tests
{
    [TestClass()]
    public class RepresentationModelTests
    {
        private EzDrinkModel _baseModel;
        private RepresentationModel _model;

        // 初始化
        [TestInitialize()]
        public void Initialize()
        {
            _baseModel = new EzDrinkModel();
            _baseModel.MakeAnOrder(0);
            _model = new RepresentationModel(_baseModel);
        }

        [TestMethod()]
        public void ModelTest()
        {
            Assert.AreEqual(_model.Model, _baseModel);
        }

        [TestMethod()]
        public void SelectDrinkTest()
        {
            DataGridViewCellEventArgs forTrue = new DataGridViewCellEventArgs(0, 1);
            DataGridViewCellEventArgs forFalse = new DataGridViewCellEventArgs(1, 1);
            Assert.IsFalse(_model.SelectDrink(forFalse));
            Assert.IsTrue(_model.SelectDrink(forTrue));
        }

        [TestMethod()]
        public void RemoveOrderClickTest()
        {
            DataGridViewCellEventArgs forTrue = new DataGridViewCellEventArgs(InitialData.DELETE_COLUMN_INDEX, 0);
            Assert.IsTrue(_model.RemoveOrderClick(forTrue));
        }

        [TestMethod()]
        public void ClickNormalSweetTest()
        {
            Assert.IsFalse(_model.ClickNormalSweet(InitialData.EMPTY_INDEX));
            Assert.IsTrue(_model.ClickNormalSweet(0));
        }

        [TestMethod()]
        public void ClickHalfSweetTest()
        {
            Assert.IsFalse(_model.ClickHalfSweet(InitialData.EMPTY_INDEX));
            Assert.IsTrue(_model.ClickHalfSweet(0));
        }

        [TestMethod()]
        public void ClickNoSweetTest()
        {
            Assert.IsFalse(_model.ClickNoSweet(InitialData.EMPTY_INDEX));
            Assert.IsTrue(_model.ClickNoSweet(0));
        }

        [TestMethod()]
        public void ClickFewSweetTest()
        {
            Assert.IsFalse(_model.ClickFewSweet(InitialData.EMPTY_INDEX));
            Assert.IsTrue(_model.ClickFewSweet(0));
        }

        [TestMethod()]
        public void ClickNormalIceTest()
        {
            Assert.IsFalse(_model.ClickNormalIce(InitialData.EMPTY_INDEX));
            Assert.IsTrue(_model.ClickNormalIce(0));
        }

        [TestMethod()]
        public void ClickHalfIceTest()
        {
            Assert.IsFalse(_model.ClickHalfIce(InitialData.EMPTY_INDEX));
            Assert.IsTrue(_model.ClickHalfIce(0));
        }

        [TestMethod()]
        public void ClickNoIceTest()
        {
            Assert.IsFalse(_model.ClickNoIce(InitialData.EMPTY_INDEX));
            Assert.IsTrue(_model.ClickNoIce(0));
        }

        [TestMethod()]
        public void ClickFewIceTest()
        {
            Assert.IsFalse(_model.ClickFewIce(InitialData.EMPTY_INDEX));
            Assert.IsTrue(_model.ClickFewIce(0));
        }

        [TestMethod()]
        public void SaveOrderToModelTest()
        {
            _model.SaveOrderToModel();
            Assert.AreEqual(_model.Model.GetAllOrder().Count, 1);
            _model.Model.SetIceOf(0, Ice.No);
            _model.Model.SetSweetOf(0, Sweet.No);
            _model.SaveOrderToModel();
            Assert.AreEqual(_model.Model.GetAllOrder().Count, 0);
        }

        [TestMethod()]
        public void OrderSelectChangeTest()
        {
            DataGridViewRowStateChangedEventArgs forFalse = new DataGridViewRowStateChangedEventArgs(new DataGridViewRow(), DataGridViewElementStates.Selected);
            Assert.IsFalse(_model.OrderSelectChange(forFalse));
            DataGridViewRowStateChangedEventArgs forTrue = new DataGridViewRowStateChangedEventArgs(new DataGridViewRow(), DataGridViewElementStates.None);
            Assert.IsTrue(_model.OrderSelectChange(forTrue));
        }

        [TestMethod()]
        public void SelectHistoryItemTest()
        {
            _model.Model.SetIceOf(0, Ice.No);
            _model.Model.SetSweetOf(0, Sweet.No);
            _model.SaveOrderToModel();
            Assert.AreEqual(_model.SelectHistoryItem(0), _baseModel.GetAllHistory()[0].GetOrder());
            Assert.AreEqual(_model.SelectHistoryItem(InitialData.EMPTY_INDEX), null);
        }

        [TestMethod()]
        public void ClickAboutTest()
        {
            const String VERSION = "V1.0";
            const String DEVELOPER = "102331020";
            const String SYSTEM_NAME = "EzDrink";
            const String LAST_UPDATED_DAY = "2016-10-18";
            const String BREAK = "\n";
            String MESSAGE = "版本:" + VERSION + BREAK + "開發者學號:" + DEVELOPER + BREAK + "系統名稱:" + SYSTEM_NAME + BREAK + "最後更新日期" + LAST_UPDATED_DAY;
            Assert.AreEqual(_model.GetAboutString(), MESSAGE);
        }

        [TestMethod()]
        public void TryAddDrinkTest()
        {
            int length = _baseModel.GetAllDrink().Count + 1;
            _model.TryAddDrink("", 1);
            Assert.AreEqual(_baseModel.GetAllDrink().Count, length);
        }

        [TestMethod()]
        public void TryAddIngredientTest()
        {
            int length = _baseModel.GetIngredient().Container.Count + 1;
            _model.TryAddIngredient("", 1);
            Assert.AreEqual(_baseModel.GetIngredient().Container.Count, length);
        }

        [TestMethod()]
        public void ClickDeleteDrinkTest()
        {
            DataGridViewCellEventArgs forTrue = new DataGridViewCellEventArgs(0, 0);
            Assert.IsTrue(_model.ClickDeleteDrink(forTrue));
        }

        [TestMethod()]
        public void ClickDeleteIngredientTest()
        {
            DataGridViewCellEventArgs forTrue = new DataGridViewCellEventArgs(0, 0);
            Assert.IsTrue(_model.ClickDeleteIngredient(forTrue));
        }

        [TestMethod()]
        public void ChangeCellValueOfDrinkGridTest()
        {
            DataGridView view = new DataGridView();
            view.ColumnCount = 3;
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            const String NAME = "1";
            const String PRICE = "123";
            String[] testData = { "" , NAME, PRICE };
            view.Rows.Add(testData);
            DataGridViewCellEventArgs forName = new DataGridViewCellEventArgs(InitialData.INDEX_FOR_TITLE, 0);
            Assert.IsTrue(_model.ChangeCellValueOfDrinkGrid(forName, view));
            Assert.AreEqual(_model.Model.GetAllDrink()[0].Title, NAME);
            DataGridViewCellEventArgs forPrice = new DataGridViewCellEventArgs(InitialData.INDEX_FOR_PRICE, 0);
            Assert.IsFalse(_model.ChangeCellValueOfDrinkGrid(forPrice, view));
            Assert.AreEqual(_model.Model.GetAllDrink()[ 0 ].Price.ToString(), PRICE);

            DataGridViewCellEventArgs forFalseColumn = new DataGridViewCellEventArgs(InitialData.EMPTY_INDEX, 0);
            Assert.IsFalse(_model.ChangeCellValueOfDrinkGrid(forFalseColumn, view));
            DataGridViewCellEventArgs forFalseRow = new DataGridViewCellEventArgs(0, InitialData.EMPTY_INDEX);
            Assert.IsFalse(_model.ChangeCellValueOfDrinkGrid(forFalseRow, view));
        }

        [TestMethod()]
        public void ChangeCellValueOfIngredientGridTest()
        {
            DataGridView view = new DataGridView();
            view.ColumnCount = 3;
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            const String NAME = "1";
            const String PRICE = "123";
            String[] testData = { "", NAME, PRICE };
            view.Rows.Add(testData);
            DataGridViewCellEventArgs forName = new DataGridViewCellEventArgs(InitialData.INDEX_FOR_TITLE, 0);
            Assert.IsTrue(_model.ChangeCellValueOfIngredientGrid(forName, view));
            Assert.AreEqual(_model.Model.GetIngredient().Container[0].TheIngredient.Title, NAME);
            DataGridViewCellEventArgs forPrice = new DataGridViewCellEventArgs(InitialData.INDEX_FOR_PRICE, 0);
            Assert.IsFalse(_model.ChangeCellValueOfIngredientGrid(forPrice, view));
            Assert.AreEqual(_model.Model.GetIngredient().Container[0].TheIngredient.Price.ToString(), PRICE);

            DataGridViewCellEventArgs forFalseColumn = new DataGridViewCellEventArgs(InitialData.EMPTY_INDEX, 0);
            Assert.IsFalse(_model.ChangeCellValueOfIngredientGrid(forFalseColumn, view));
            DataGridViewCellEventArgs forFalseRow = new DataGridViewCellEventArgs(0, InitialData.EMPTY_INDEX);
            Assert.IsFalse(_model.ChangeCellValueOfIngredientGrid(forFalseRow, view));
        }

        [TestMethod()]
        public void SelectIngredientTest()
        {
            DataGridViewCellEventArgs forTrue = new DataGridViewCellEventArgs(0, 0);
            Assert.IsTrue(_model.SelectIngredient(forTrue, 0));
            DataGridViewCellEventArgs forFalseColumn = new DataGridViewCellEventArgs(1, 0);
            Assert.IsFalse(_model.SelectIngredient(forTrue, InitialData.EMPTY_INDEX));
            Assert.IsFalse(_model.SelectIngredient(forFalseColumn, 0));
        }
    }
}