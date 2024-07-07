using UnityEngine;
using UnityEngine.UI;
using System;

public class CampaignInventoryManager : MonoBehaviour
{
    private int tokens;
    [SerializeField] Text tokensText;
    [SerializeField] Text secondWeapon;
    [SerializeField] Text thirdWeapon;
    [SerializeField] WeaponSwitch weaponSwitch;

    [SerializeField] WeaponSelectSO[] weaponSelectSO;
    [SerializeField] WeaponSelectTemplate[] weaponPanels;
    [SerializeField] Button[] purchaseButtons;
    [SerializeField] Text[] purchaseButtonsText;
    private bool[] activated = {false,false,false,false,false,false,false,false};
    private const int AR_INDEX = 1;
    private const int SNIPER_INDEX = 2;
    private const int SMG_INDEX = 3;
    private const int SHOTGUN_INDEX = 4;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0f;
        PauseMenu.gameIsPaused = true;
    }


    private void Start()
    {
        if(secondWeapon != null && thirdWeapon != null && weaponSwitch != null && StateManagement.Instance != null)
        {
            tokens = StateManagement.Instance.GetTokens();
            tokensText.text = tokens.ToString();
            tokens = 1000;
            secondWeapon.text = "Second Weapon: Not Selected";
            thirdWeapon.text = "Third Weapon: Not Selected";
            weaponSwitch.OnWeaponSet += OnWeaponChanged;
        }
        LoadPanels();
        CheckPurchaseable();
    }

    private void OnDisable()
    {
        weaponSwitch.OnWeaponSet -= OnWeaponChanged;
    }

    private void OnWeaponChanged(object sender, EventArgs e)
    {
        if(weaponSwitch == null)
        {
            Debug.LogError("WeaponSwitch is null in CampaignInventoryManager");
            return;
        }
        if(secondWeapon == null || thirdWeapon == null)
        {
            Debug.LogError("Second or Third Weapon Text is null in CampaignInventoryManager");
            return;
        }
        secondWeapon.text = "Second Weapon: " + weaponSwitch.GetSecondWeaponName();
        thirdWeapon.text = "Third Weapon: " + weaponSwitch.GetThirdWeaponName();
    
    }

    public void Close()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.gameIsPaused = false;
    }

    private void CheckPurchaseable()
    {
        for(int i = 0; i < weaponSelectSO.Length; i++)
        {
            string name = weaponSelectSO[i].weaponName;
            bool isUnlocked = false;
            if(StateManagement.Instance != null)
            {
                isUnlocked = StateManagement.Instance.IsUnlocked(name);
            }
            if(activated[i])
            {
                purchaseButtonsText[i].text = "SELECT";
                purchaseButtons[i].interactable = true;
            }
            else if(isUnlocked && tokens >= weaponSelectSO[i].tokenCost)
            {
                purchaseButtonsText[i].text = "ACTIVATE";
                purchaseButtons[i].interactable = true;
            }
            else if(isUnlocked && tokens < weaponSelectSO[i].tokenCost)
            {
                purchaseButtons[i].interactable = false;
            }
            else
            {
                purchaseButtonsText[i].text = "LOCKED";
                purchaseButtons[i].interactable = false;
            }
        }
    
    }
    private void LoadPanels()
    {
        for(int i = 0; i < weaponSelectSO.Length; i++)
        {
            weaponPanels[i].SetShopItem(weaponSelectSO[i]);
        }
    }

    public void HandleSecondSlot(int weaponIndex)
    {
        int arrayIndex = -1;
        if(weaponIndex == AR_INDEX)
        {
            arrayIndex = 1;
        }
        else if(weaponIndex == SNIPER_INDEX)
        {
            arrayIndex = 3;
        }
        else if(weaponIndex == SMG_INDEX)
        {
            arrayIndex = 0;
        }
        else if(weaponIndex == SHOTGUN_INDEX)
        {
            arrayIndex = 2;
        }

        if(arrayIndex == -1) return;

        if(activated[arrayIndex])
        {
            weaponSwitch.SetSecondWeapon(weaponIndex);
            return;
        }
        if(StateManagement.Instance != null)
        {
            tokens -= weaponSelectSO[arrayIndex].tokenCost;
            tokensText.text = tokens.ToString();
            activated[arrayIndex] = true;
            activated[arrayIndex + 4] = true;
            weaponSwitch.SetSecondWeapon(weaponIndex);
            CheckPurchaseable();
        }
    }

    public void HandleThirdSlot(int weaponIndex)
    {
        int arrayIndex = -1;
        if(weaponIndex == AR_INDEX)
        {
            arrayIndex = 1;
        }
        else if(weaponIndex == SNIPER_INDEX)
        {
            arrayIndex = 3;
        }
        else if(weaponIndex == SMG_INDEX)
        {
            arrayIndex = 0;
        }
        else if(weaponIndex == SHOTGUN_INDEX)
        {
            arrayIndex = 2;
        }

        if(arrayIndex == -1) return;

        if(activated[arrayIndex])
        {
            weaponSwitch.SetThirdWeapon(weaponIndex);
            return;
        }
        if(StateManagement.Instance != null)
        {
            tokens -= weaponSelectSO[arrayIndex].tokenCost;
            tokensText.text = tokens.ToString();
            activated[arrayIndex] = true;
            activated[arrayIndex + 4] = true;
            weaponSwitch.SetThirdWeapon(weaponIndex);
            CheckPurchaseable();
        }
    }
}
