using System;
[Serializable]
public struct SaveInfo
{
    public GameScene currentScene;
    public int chamber;
    public int slot;
    public DateTime lastSaved;
    public CharacterType character;
    public Difficulty difficulty;
    public SaveInfo(GameScene scene, int chamber, int slot, DateTime date,CharacterType character,Difficulty difficulty)
    {
        this.currentScene = scene;
        this.chamber = chamber;
        this.slot = slot;
        this.lastSaved = date;
        this.character = character;
        this.difficulty = difficulty;
    }
}
