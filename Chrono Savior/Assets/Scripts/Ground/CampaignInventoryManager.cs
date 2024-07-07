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
    private bool[] activated = { false, false, false, false, false, false, false, false };
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
        if (tokensText == null || secondWeapon == null || thirdWeapon == null || weaponSwitch == null || StateManagement.Instance == null)
        {
            Debug.LogError("One or more serialized fields or StateManagement.Instance are not assigned in CampaignInventoryManager.");
            return;
        }

        tokens = StateManagement.Instance.GetTokens();
        tokensText.text = tokens.ToString();
        secondWeapon.text = "Second Weapon: Not Selected";
        thirdWeapon.text = "Third Weapon: Not Selected";
        weaponSwitch.OnWeaponSet += OnWeaponChanged;

        LoadPanels();
        CheckPurchaseable();
    }

    private void OnDisable()
    {
        if (weaponSwitch != null)
        {
            weaponSwitch.OnWeaponSet -= OnWeaponChanged;
        }
    }

    private void OnWeaponChanged(object sender, EventArgs e)
    {
        if (weaponSwitch == null)
        {
            Debug.LogError("WeaponSwitch is null in CampaignInventoryManager");
            return;
        }
        if (secondWeapon == null || thirdWeapon == null)
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
        for (int i = 0; i < weaponSelectSO.Length; i++)
        {
            string name = weaponSelectSO[i].weaponName;
            bool isUnlocked = false;
            if (StateManagement.Instance != null)
            {
                isUnlocked = StateManagement.Instance.IsUnlocked(name);
            }
            if (activated[i])
            {
                if (purchaseButtonsText != null && purchaseButtonsText[i] != null)
                {
                    purchaseButtonsText[i].text = "SELECT";
                }
                if (purchaseButtons != null && purchaseButtons[i] != null)
                {
                    purchaseButtons[i].interactable = true;
                }
            }
            else if (isUnlocked && tokens >= weaponSelectSO[i].tokenCost)
            {
                if (purchaseButtonsText != null && purchaseButtonsText[i] != null)
                {
                    purchaseButtonsText[i].text = "ACTIVATE";
                }
                if (purchaseButtons != null && purchaseButtons[i] != null)
                {
                    purchaseButtons[i].interactable = true;
                }
            }
            else if (isUnlocked && tokens < weaponSelectSO[i].tokenCost)
            {
                if (purchaseButtons != null && purchaseButtons[i] != null)
                {
                    purchaseButtons[i].interactable = false;
                }
            }
            else
            {
                if (purchaseButtonsText != null && purchaseButtonsText[i] != null)
                {
                    purchaseButtonsText[i].text = "LOCKED";
                }
                if (purchaseButtons != null && purchaseButtons[i] != null)
                {
                    purchaseButtons[i].interactable = false;
                }
            }
        }
    }

    private void LoadPanels()
    {
        for (int i = 0; i < weaponSelectSO.Length; i++)
        {
            if (weaponPanels != null && weaponPanels[i] != null)
            {
                weaponPanels[i].SetShopItem(weaponSelectSO[i]);
            }
            else
            {
                Debug.LogError($"Weapon panel at index {i} is null in CampaignInventoryManager");
            }
        }
    }

    public void HandleSecondSlot(int weaponIndex)
    {
        int arrayIndex = GetWeaponArrayIndex(weaponIndex);
        if (arrayIndex == -1) return;

        if (weaponSwitch == null || tokensText == null)
        {
            Debug.LogError("WeaponSwitch or tokensText is null in CampaignInventoryManager");
            return;
        }

        if (activated[arrayIndex])
        {
            weaponSwitch.SetSecondWeapon(weaponIndex);
            return;
        }
        if (StateManagement.Instance != null)
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
        int arrayIndex = GetWeaponArrayIndex(weaponIndex);
        if (arrayIndex == -1) return;

        if (weaponSwitch == null || tokensText == null)
        {
            Debug.LogError("WeaponSwitch or tokensText is null in CampaignInventoryManager");
            return;
        }

        if (activated[arrayIndex])
        {
            weaponSwitch.SetThirdWeapon(weaponIndex);
            return;
        }
        if (StateManagement.Instance != null)
        {
            tokens -= weaponSelectSO[arrayIndex].tokenCost;
            tokensText.text = tokens.ToString();
            activated[arrayIndex] = true;
            activated[arrayIndex + 4] = true;
            weaponSwitch.SetThirdWeapon(weaponIndex);
            CheckPurchaseable();
        }
    }

    private int GetWeaponArrayIndex(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case AR_INDEX:
                return 1;
            case SNIPER_INDEX:
                return 3;
            case SMG_INDEX:
                return 0;
            case SHOTGUN_INDEX:
                return 2;
            default:
                return -1;
        }
    }
}
