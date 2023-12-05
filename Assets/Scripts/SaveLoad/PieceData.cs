namespace SNG.Save
{
    [System.Serializable]
    public class PieceData
    {
        public int CurrentPieceIndex;
        public bool[] Purchased = new bool[4];

        public PieceData()
        {
            CurrentPieceIndex = 0;
            Purchased[0] = true;
            for (int i=1; i<4; i++)
            {
                Purchased[i] = false;
            }
        }

        public void ChangeSpriteIndex(int newIndex)
        {
            CurrentPieceIndex = newIndex;
        }

        public void ChangePurchaseStatus(int index, bool newStatus)
        {
            Purchased[index] = newStatus;
        }
    }
}