using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject menusWrap;
    public GameObject HUD;
    public GameObject[] tooltips;
    public GameObject[] menus;

    private GameObject currentMenu;

    private bool MenusShow = false;

    void Start()
    {
        menusWrap.SetActive(MenusShow);

        currentMenu = menus[0]; // Set the first menu as the current menu
        currentMenu.SetActive(true); // Activate the first menu

        for (int i = 1; i < menus.Length; i++)
        {
            menus[i].SetActive(false); // Deactivate all other menus
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenusShow = !MenusShow;
            menusWrap.SetActive(MenusShow);
            HUD.SetActive(!MenusShow);

            if (!MenusShow)
            {
                foreach (var tooltip in tooltips)
                {
                    tooltip.SetActive(false);
                }
            }
        }
    }

    public void SwitchMenu(GameObject menu)
    {
        currentMenu.SetActive(false);

        currentMenu = menu;

        currentMenu.SetActive(true);
    }
}
