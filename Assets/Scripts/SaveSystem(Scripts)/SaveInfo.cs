using System;
[Serializable]
public struct SaveInfo
{
    public GameScene currentScene;
    public int chamber;
    public int slot;
    public DateTime lastSaved;
    public SaveInfo(GameScene scene, int chamber, int slot, DateTime date)
    {
        this.currentScene = scene;
        this.chamber = chamber;
        this.slot = slot;
        this.lastSaved = date;
    }
}
