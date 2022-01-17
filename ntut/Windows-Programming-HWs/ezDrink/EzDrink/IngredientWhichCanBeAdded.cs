using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDrink
{
    /*
     * 可以區分有沒有被加的配料的資料型別
     */
    public class IngredientWhichCanBeAdded : ICloneable, ISame<IngredientWhichCanBeAdded>
    {
        private Ingredient _ingredient;
        private bool _added;

        public IngredientWhichCanBeAdded(Ingredient top)
        {
            _ingredient = top;
            _added = false;
        }

        // 回傳配料
        public Ingredient TheIngredient
        {
            get
            {
                return _ingredient;
            }
        }

        // 被加了嗎
        public bool IsAdded
        {
            get
            {
                return _added;
            }
            set
            {
                _added = value;
            }
        }

        // 配料
        public Ingredient Top
        {
            get
            {
                return _ingredient;
            }
        }

        // 實做ICloneable
        public object Clone()
        {
            return new IngredientWhichCanBeAdded((Ingredient)Top.Clone());
        }

        // 是不是一樣的配料
        public bool IsSame(IngredientWhichCanBeAdded item)
        {
            return TheIngredient.IsSame(item.TheIngredient);
        }
    }
}
