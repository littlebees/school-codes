using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzDrink
{
    public class AddingButtonsStateMachine<T> where T : ISame<T>
    {
        public delegate void FileHookEventHandler();
        public delegate void TextHookEventHandler(String name, int price);

        private const String START_FOLDER = "c:\\";
        private const string READ_FILE_TYPE = "txt files (*.txt)|*.txt";
        private FileHookEventHandler _fileHook;
        private TextHookEventHandler _textHook;
        private const string NOT_DEFINE_STATE_EXCEPTION_MESSAGE = "Unspecific state";
        private const string FILE_BUTTON_TEXT = "從檔案匯入";
        private const string TEXT_BOX_BUTTON_TEXT = "新增";
        private const string CANCEL_BUTTON_TEXT = "取消";
        private const string OK_BUTTON_TEXT = "確定";
        private const string IMPOSSIBLE_STATE_TRANSFER = "This is impossible state change";
        private Button _fromTextBox;
        private Button _fromFile;
        private TextBox _price;
        private TextBox _name;
        private AddingButtonsState _state;
        private EzDrinkModel _model;

        public AddingButtonsStateMachine(Button fromTextBox, Button fromFile, TextBox name, TextBox price, TextHookEventHandler readText, EzDrinkModel model)
        {
            _fromFile = fromFile;
            _fromTextBox = fromTextBox;
            _price = price;
            _name = name;
            _state = AddingButtonsState.Initial;
            _fileHook = ReadFile;
            _textHook = readText;
            _model = model;
        }

        // 狀態轉移
        private void TransferState(AddingButtonsState nextState)
        {
            _state = nextState;
            DoSomethingInThis();
        }

        // 每個狀態該做的事
        private void DoSomethingInThis()
        {
            if (_state == AddingButtonsState.Initial)
                DoInInitial();
            else if (_state == AddingButtonsState.ClickAddFromFile)
                DoInClickAddFromFile();
            else if (_state == AddingButtonsState.ClickAddFromTextBox)
                DoInClickAddFromTextBox();
            else if (_state == AddingButtonsState.ClickOk)
                DoInClickOk();
            else if (_state == AddingButtonsState.ClickCancel)
                ResetState();
            else
                throw new Exception(NOT_DEFINE_STATE_EXCEPTION_MESSAGE);
        }

        // 當狀態是ClickAddFromFile該做?
        private void DoInClickAddFromFile()
        {
            _fileHook();
            ResetState();
        }

        // 當狀態是ClickOk該做?
        private void DoInClickOk()
        {
            try
            {
                _textHook(_name.Text, int.Parse(_price.Text));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                ResetState();
            }
        }

        // 當狀態是ClickAddFromTextBox該做?
        private void DoInClickAddFromTextBox()
        {
            _price.Enabled = true;
            _name.Enabled = true;
            _fromFile.Text = CANCEL_BUTTON_TEXT;
            _fromTextBox.Text = OK_BUTTON_TEXT;
        }

        // 當狀態是ClickAddFromFile該做?
        private void DoInInitial()
        {
            _price.Text = "";
            _name.Text = "";
            _price.Enabled = false;
            _name.Enabled = false;
            _fromFile.Text = FILE_BUTTON_TEXT;
            _fromTextBox.Text = TEXT_BOX_BUTTON_TEXT;
        }

        // 按下由TextBox新增的按鈕時
        public void ClickTextBoxButton()
        {
            AddingButtonsState nextState = AddingButtonsState.Initial;
            switch (_state)
            {
                case AddingButtonsState.Initial:
                    nextState = AddingButtonsState.ClickAddFromTextBox;
                    break;
                case AddingButtonsState.ClickAddFromTextBox:
                    nextState = AddingButtonsState.ClickOk;
                    break;
                default:
                    throw new Exception(IMPOSSIBLE_STATE_TRANSFER);
            }
            TransferState(nextState);
        }

        // 按下由File新增的按鈕時
        public void ClickFileButton()
        {
            AddingButtonsState nextState = AddingButtonsState.Initial;
            switch (_state)
            {
                case AddingButtonsState.Initial:
                    nextState = AddingButtonsState.ClickAddFromFile;
                    break;
                case AddingButtonsState.ClickAddFromTextBox:
                    nextState = AddingButtonsState.ClickCancel;
                    break;
                default:
                    throw new Exception(IMPOSSIBLE_STATE_TRANSFER);
            }
            TransferState(nextState);
        }

        // 變成初始狀態
        private void ResetState()
        {
            TransferState(AddingButtonsState.Initial);
        }

        // 讀檔案的函數
        private void ReadFile()
        {
            OpenFileDialog openFileDialog = MakeFileObject();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                try
                {
                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        //DataContainer<T> storeData = _model.AddNewItem<T>(reader);
                        if (typeof(T) == typeof(Drink))
                            _model.GetAdapterCollection().UpdateDrink(_model.GetAllDrink());
                        else if (typeof(T) == typeof(IngredientWhichCanBeAdded))
                            _model.GetAdapterCollection().UpdateForAddNewIngredient(_model.AddNewItem<T>(reader).Container.Select(item => (item as IngredientWhichCanBeAdded).TheIngredient).ToList());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        // openfile的物件生成
        private OpenFileDialog MakeFileObject()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = START_FOLDER;
            openFileDialog.Filter = READ_FILE_TYPE;
            openFileDialog.FilterIndex = 1;
            return openFileDialog;
        }
    }
}
