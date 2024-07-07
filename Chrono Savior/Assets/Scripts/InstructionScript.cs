using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsPage : MonoBehaviour
{
    public Text instructionsText;

    void Start()
    {
        instructionsText.text =
            "\n" +
            "<b><size=60><color=#000000>Movement</color></size></b>\n" +
            "<b>Keyboard:</b>\n" +
            "<i>• Ship:</i> Use the <b><color=#FF0000>W</color></b> and <b><color=#FF0000>S</color></b> keys to move the ship.\n" +
            "<i>• Character:</i> Use <b><color=#FF0000>W</color></b>, <b><color=#FF0000>A</color></b>, <b><color=#FF0000>S</color></b>, <b><color=#FF0000>D</color></b> keys to move the character in space and ground modes.\n\n\n" +
            "<b><size=60><color=#000000>Aiming and Shooting</color></size></b>\n" +
            "<b>Mouse:</b>\n" +
            "<i>• Aim:</i> Use the mouse cursor to aim.\n" +
            "<i>• Shoot:</i> Left-click to shoot.\n\n\n" +
            "<b><size=60><color=#000000>Special Abilities</color></size></b>\n" +
            "<b>Keyboard:</b>\n" +
            "<i>• Press <b><color=#FF0000>Z</color></b> to activate special abilities.</i>\n\n\n" +
            "<b><size=60><color=#000000>Pause Menu</color></size></b>\n" +
            "<b>Keyboard:</b>\n" +
            "<i>• Press <b><color=#FF0000>Esc</color></b> to open the pause menu.</i>\n\n\n" +
            "<b><size=60><color=#000000>Weapon Swapping and Reload</color></size></b>\n" +
            "<b>Keyboard:</b>\n" +
            "<i>• Press <b><color=#FF0000>1</color></b>, <b><color=#FF0000>2</color></b>, <b><color=#FF0000>3</color></b> to change weapons.</i>\n" +
            "<i>• Press <b><color=#FF0000>R</color></b> to reload.</i>\n" +
            "\n\n";
    }
}
