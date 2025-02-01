namespace Assets.Scripts.Save
{
    public interface ISaveable
    {
        public void SaveData(SaveData data);
        public void LoadData(SaveData data);
    }
}