using UnityEngine;
using System.Collections;

public class FCharacterSelectData 
{
    private static int PlayerCount = 1;

    private int CurrentTextureIndex = 0;

    private bool bEnabled;
	private bool bIsReady;
	private bool bIsKeyboard;

    public Texture2D SelectedCharacterTexture;

    public string PlayerTitle;

    public FCharacterSelectData()
    {
        if (PlayerCount < 5)
        {
            PlayerTitle = "Player " + PlayerCount.ToString();

            SelectedCharacterTexture = FCharacterTextures.CharacterTextures[CurrentTextureIndex];

            ++PlayerCount;

            bEnabled = false;
        }
    }

    public void Enable()
    {
        bEnabled = true;
    }

    public bool IsEnabled()
    {
        return bEnabled;
    }

    public void Disable()
    {
        bEnabled = false;
    }

	public bool IsReady()
	{
		return bIsReady;
	}

	public void IsReady(bool bIsReady)
	{
		this.bIsReady = bIsReady;
	}

	public bool IsKeyboard()
	{
		return bIsKeyboard;
	}

	public void IsKeyboard(bool bIsKeyboard)
	{
		this.bIsKeyboard = bIsKeyboard;
	}

	public bool IsXBoxController()
	{
		return !bIsKeyboard;
	}

    public void NextTexture()
    {
        if (++CurrentTextureIndex >= FCharacterTextures.CharacterTextures.Length)
        {
            CurrentTextureIndex = 0;
        }

        SelectedCharacterTexture = FCharacterTextures.CharacterTextures[CurrentTextureIndex];
    }

    public void PrevTexture()
    {
        if (--CurrentTextureIndex < 0)
        {
            CurrentTextureIndex = FCharacterTextures.CharacterTextures.Length - 1;
        }

        SelectedCharacterTexture = FCharacterTextures.CharacterTextures[CurrentTextureIndex];
    }
}
