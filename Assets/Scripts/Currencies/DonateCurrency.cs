namespace Game.Currencies
{
    public abstract class DonateCurrency : Currency
    {
        public override bool Spend(int cost)
        {
            if (Amount >= cost)
            {
                Amount -= cost;
                UpdateUI();
                return true;
            }

            return false;
        }

        public override void Earn(int value)
        {
            Amount += value;
            UpdateUI();
        }

        public virtual void Buy()
        {
            //the logic of the purchase should be here
        }
    }
}
