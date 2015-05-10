using UnityEngine;
using System.Collections;

public struct FPlayerIndex
{
    public const int PlayerOne = 0;
    public const int PlayerTwo = 1;
    public const int PlayerThree = 2;
    public const int PlayerFour = 3;
}

public struct FControllerIndex
{
	public const int Keyboard = 0;
	public const int XBoxController = 1;
}

public struct FXboxControllerButton
{
	public const int Button_A = 0;
	public const int Button_B = 1;
	public const int Button_X = 2;
	public const int Button_Y = 3;
	public const int Button_LB = 4;
	public const int Button_RB = 5;
	public const int Button_Back = 6;
	public const int Button_Start = 7;
}

public struct FCharacterTextures
{
    public struct FCharacterTextureIndex
    {
        public const int FoxCharacter = 0;
        public const int PigCharacter = 1;
    }

    public static Texture2D[] CharacterTextures = {
                                                      Resources.Load<Sprite>("2D/Images/fox_image").texture,
                                                      Resources.Load<Sprite>("2D/Images/pig_image").texture
                                                  };
}