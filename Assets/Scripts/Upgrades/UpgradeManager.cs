using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class PlayerUpgradeManager : MonoBehaviour
{
    [System.Serializable]
    public class Upgrade
    {
        public string upgradeName;
        public string description;
        public System.Action applyUpgrade;
    }

    [Header("UI References")]
    public GameObject upgradePanel;
    public RectTransform card1Rect;
    public RectTransform card2Rect;
    public Button card1Button;
    public Button card2Button;
    public TextMeshProUGUI card1NameText;
    public TextMeshProUGUI card1DescText;
    public TextMeshProUGUI card2NameText;
    public TextMeshProUGUI card2DescText;

    [Header("Kill Milestones")]
    public int[] killMilestones = { 1, 25, 50, 100, 250, 500, 1000 };

    [Header("All Upgrades (Ordered)")]
    public List<Upgrade> allUpgrades = new List<Upgrade>();

    private int currentMilestoneIndex = 0;
    private int currentUpgradeIndex = 0;
    private bool upgradePending = false;
    private Upgrade[] currentOptions = new Upgrade[2];

    private Spawner spawner;

    void Awake()
    {
        spawner = FindFirstObjectByType<Spawner>();

        // ===== ORDER MATTERS HERE =====

        allUpgrades.Add(new Upgrade {
            upgradeName = "Double Lasers",
            description = "Adds a second bullet spawner",
            applyUpgrade = () => PlayerController.Instance.AddBulletSpawner()
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Faster Fire",
            description = "Reduces cooldown between shots",
            applyUpgrade = () => PlayerController.Instance.ReduceFireCooldown(0.35f)
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Stronger Lasers",
            description = "Increases laser damage by 5",
            applyUpgrade = () => PlayerController.Instance.IncreaseLaserDamage(5)
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Triple Lasers",
            description = "Adds a third bullet spawner",
            applyUpgrade = () => PlayerController.Instance.AddBulletSpawner()
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Rapid Fire",
            description = "Reduces cooldown further",
            applyUpgrade = () => PlayerController.Instance.ReduceFireCooldown(0.2f)
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "High Damage",
            description = "Increase laser damage by 10",
            applyUpgrade = () => PlayerController.Instance.IncreaseLaserDamage(10)
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Ultimate Fire Rate",
            description = "Significantly reduces cooldown",
            applyUpgrade = () => PlayerController.Instance.ReduceFireCooldown(0.1f)
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Laser Overload",
            description = "Greatly increases laser damage",
            applyUpgrade = () => PlayerController.Instance.IncreaseLaserDamage(20)
        });
    }

    void Update()
    {
        if (upgradePending) return;

        if (currentMilestoneIndex < killMilestones.Length &&
            PlayerController.Instance.killCounter >= killMilestones[currentMilestoneIndex])
        {
            ShowUpgradePanel();
            currentMilestoneIndex++;
        }
    }

    void ShowUpgradePanel()
    {
        if (currentUpgradeIndex >= allUpgrades.Count)
            return; // No upgrades left

        spawner.spawningEnabled = false;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);

        upgradePending = true;
        upgradePanel.SetActive(true);

        // ===== ORDERED SELECTION =====
        currentOptions[0] = allUpgrades[currentUpgradeIndex];

        if (currentUpgradeIndex + 1 < allUpgrades.Count)
            currentOptions[1] = allUpgrades[currentUpgradeIndex + 1];
        else
            currentOptions[1] = null;

        // Animate
        card1Rect.localScale = Vector3.zero;
        card2Rect.localScale = Vector3.zero;

        upgradePanel.transform.localPosition = new Vector3(0, 600, 0);
        upgradePanel.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);
        card1Rect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f);
        card2Rect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.4f);

        // Update UI
        card1NameText.text = currentOptions[0].upgradeName;
        card1DescText.text = currentOptions[0].description;

        if (currentOptions[1] != null)
        {
            card2NameText.text = currentOptions[1].upgradeName;
            card2DescText.text = currentOptions[1].description;
            card2Button.gameObject.SetActive(true);
        }
        else
        {
            card2Button.gameObject.SetActive(false);
        }

        card1Button.onClick.RemoveAllListeners();
        card2Button.onClick.RemoveAllListeners();

        card1Button.onClick.AddListener(() => PickUpgrade(0));
        card2Button.onClick.AddListener(() => PickUpgrade(1));
    }

    void PickUpgrade(int chosenIndex)
    {
        if (currentOptions[chosenIndex] == null)
            return;

        currentOptions[chosenIndex].applyUpgrade?.Invoke();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.upgradeUnlockSFX);

        currentUpgradeIndex += 2; // Move forward in order

        Sequence seq = DOTween.Sequence();
        seq.Append(card1Rect.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack));
        seq.Join(card2Rect.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack));
        seq.Append(upgradePanel.transform.DOLocalMoveY(600, 0.3f).SetEase(Ease.InBack));
        seq.OnComplete(() =>
        {
            upgradePanel.SetActive(false);
            upgradePending = false;
        });

        spawner.spawningEnabled = true;
    }
}