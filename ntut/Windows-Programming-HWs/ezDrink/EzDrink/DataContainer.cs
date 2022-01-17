using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    public class DataContainer<T> where T : ISame<T>
    {
        public delegate void ContainerEventHandler(List<T> datas);
        private List<T> _container = new List<T>();
        private const char SPLIT_STRING = ' ';
        private const int LEGAL_WORD = 2;
        private const string ERROR_FORMAT = "Error format";
        private const string NO_LEGAL_INPUT_FOR_PRICE = "price must be int";
        private String _duplicateString;

        public DataContainer(ContainerEventHandler function, String duplicateString)
        {
            function(_container);
            _duplicateString = duplicateString;
        }

        //list的getter
        public List<T> Container
        {
            get
            {
                return _container;
            }
        }

        // 用index得到東西
        public T GetItem(int index)
        {
            return _container[index];
        }

        // 用index set 東西
        public void SetItem(int index, T item)
        {
            _container[index] = item;
        }

        // 用index remove 東西
        public void RemoveAt(int index)
        {
            _container.RemoveAt(index);
        }

        // 從尾巴加東西
        public void Add(T item)
        {
            _container.Add(item);
        }

        // 用stream批次加東西
        public DataContainer<T> AddNewItem(StreamReader stream)
        {
            DataContainer<T> storeData = new DataContainer<T>(DoNothing, _duplicateString);
            while (!stream.EndOfStream)
            {
                String line = stream.ReadLine();
                T item = ProcessString(line);

                Add(item);
                storeData.Add(item);
            }
            return storeData;
        }

        // 看有沒有重複再加
        public void TryAddItem(T input)
        {
            foreach (T item in _container)
                if (item.IsSame(input))
                    throw new Exception(_duplicateString);
            Add(input);
        }

        // 就是parse
        public T ProcessString(String rawData)
        {
            string[] tokens = rawData.Split(SPLIT_STRING);

            if (IsLegalTokens(tokens))
                return TurnTo(tokens);
            else
                throw new Exception(ERROR_FORMAT);
        }

        // 是不是合法的語法格式
        public bool IsLegalTokens(string[] tokens)
        {
            return tokens.Count() == LEGAL_WORD;
        }

        // token轉成data type
        public T TurnTo(string[] tokens)
        {
            int price = 0;
            try
            {
                price = int.Parse(tokens[1]);
            }
            catch (Exception)
            {
                throw new Exception(NO_LEGAL_INPUT_FOR_PRICE);
            }

            if (typeof(T) == typeof(IngredientWhichCanBeAdded))
                return (T)Activator.CreateInstance(typeof(T), new Ingredient(price, tokens[0]));
            else
                return (T)Activator.CreateInstance(typeof(T), price, tokens[0]);
        }

        // 什麼事都不做
        private void DoNothing(List<T> list)
        {

        }
    }
}
