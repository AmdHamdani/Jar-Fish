using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FishInterface : MonoBehaviour {
    
    #region variable
    [Header("Windows")]
    public GameObject welcomeWindow;
    public GameObject resourcesWindow;
    public GameObject monitoringWindow;
    public GameObject logWindow;
    public GameObject exploreWindow;
    public GameObject explorationDetailWindow;
    public GameObject memberWindow;
    public GameObject expLogWindow;
    public GameObject searchWindow;
    public GameObject eventLogWindow;
    public GameObject repairLogWindow;
    public GameObject endWindow;
    public GameObject explorationDetailEngineerWindow;
    public GameObject explorationDetailChefWindow;
    public GameObject exitWindow;
    [Header("Icons")]
    public GameObject resIcon;
    public GameObject logIcon;
    public GameObject monIcon;
    public GameObject endDayIcon;
    public GameObject exploreIcon;
    public GameObject memberIcon;
    public GameObject searchIcon;
    public GameObject shutdown;
    [Header("Properties")]
    public Text logText;
    public Text resText;
    public Text exploreText;
    public Text engineerText;
    public Text chefText;
    public Text explorerText;
    public Text peopleText;
    public Text expLogText;
    public Text eventLogText;
    public Text repairText;
    public Text endlogText;
    public Text engineerPeopleText;
    public Text chefPeopleText;
    public GameObject endDayPopUp;
    #endregion

    #region method
    private JarFish jarfish;

    void Start() {
        jarfish = GetComponent<JarFish>();
        jarfish.SearchOtherPlanet = true;
    }

    public void Open(string win) {
        switch (win) {
            case "resources" :
                resourcesWindow.SetActive(true);
                resText.text = jarfish.ResourceLog();
                resIcon.GetComponent<Button>().enabled = false;
                break;
            case "monitoring" : 
                monitoringWindow.SetActive(true);
                monIcon.GetComponent<Button>().enabled = false;
                break;
            case "log" : 
                logWindow.SetActive(true);
                logIcon.GetComponent<Button>().enabled = false;
                break;
            case "explore" :


                // exploreText.text = jarfish.CreatePlanet();
                // exploreWindow.SetActive(true);
                // exploreIcon.GetComponent<Button>().enabled = false;
                break;
            case "search" : 
                SearchPlanet();
                searchWindow.SetActive(true);
                searchIcon.GetComponent<Button>().enabled = false;
                if(!jarfish.SearchOtherPlanet) {
                    exploreWindow.SetActive(true);
                    exploreIcon.GetComponent<Button>().enabled = false;
                }
                break;
            case "end" : 
                endDayPopUp.SetActive(true);
                endDayIcon.GetComponent<Button>().enabled = false;
                break;
            case "member" :
                engineerText.text = jarfish.Engineer;
                chefText.text = jarfish.Chef;
                explorerText.text = jarfish.Explorer;
                memberWindow.SetActive(true);
                memberIcon.GetComponent<Button>().enabled = false;
                break;
            case "shutdown" :
                // Application.Quit();
                exitWindow.SetActive(true);
                break;
        }
        jarfish.PlaySelectSound();
    }

    public void Close(string win) {
        switch (win) {
            case "welcome" : 
                welcomeWindow.SetActive(false);
                break;
            case "resources" :
                resourcesWindow.SetActive(false);
                resIcon.GetComponent<Button>().enabled = true;
                break;
            case "monitoring" : 
                monitoringWindow.SetActive(false);
                monIcon.GetComponent<Button>().enabled = true;
                break;
            case "explore" :
                exploreWindow.SetActive(false);
                exploreIcon.GetComponent<Button>().enabled = true;
                break;
            case "log" : 
                logWindow.SetActive(false);
                logIcon.GetComponent<Button>().enabled = true;
                break;
            case "member" :
                memberWindow.SetActive(false);
                memberIcon.GetComponent<Button>().enabled = true;
                break;
            case "explore detail" :
                explorationDetailWindow.SetActive(false);
                exploreIcon.GetComponent<Button>().enabled = true;
                break;
            case "explore detail engineer" : 
                explorationDetailEngineerWindow.SetActive(false);
                exploreIcon.GetComponent<Button>().enabled = true;
                break;
            case "explore detail chef" : 
                explorationDetailChefWindow.SetActive(false);
                exploreIcon.GetComponent<Button>().enabled = true;
                break;
            case "explore log" :
                expLogWindow.SetActive(false);
                break;
            case "search" :
                searchWindow.SetActive(false);
                searchIcon.GetComponent<Button>().enabled = true;
                break;
            case "event" :
                eventLogWindow.SetActive(false);
                break;
            case "repair" :
                repairLogWindow.SetActive(false);
                break;
            case "exit" :
                exitWindow.SetActive(false);
                break;
            case "end" :
                endDayPopUp.SetActive(false);
                endDayIcon.GetComponent<Button>().enabled = true;
                break;
        }
        jarfish.PlaySelectSound();
    }

    public void RestartGame() {
        jarfish.PlaySelectSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Application.Quit();
    }

    public void ExitGame() {
        jarfish.PlaySelectSound();
        Application.Quit();
    }

    public void SearchPlanet() {
        jarfish.PlaySelectSound();
        jarfish.SearchPlanet();

        searchWindow.SetActive(false);
        searchIcon.GetComponent<Button>().enabled = false;
    }

    public void EndDay() {
        jarfish.PlaySelectSound();
        Close("end");
        jarfish.deltaDay++;
        // game over
        if(jarfish.StillAlive || jarfish.IsSpaceshipBroken) {
            endlogText.text += " System or Human died.\n";
            endlogText.text += jarfish.Logger();
            endlogText.text += " Alive for " + jarfish.deltaDay + " days.\n";
            endWindow.SetActive(true);
            return;
        }

        logText.text = jarfish.Logger();
        logWindow.SetActive(true);
        logIcon.GetComponent<Button>().enabled = false;

        // Debug.Log(jarfish.DaysToFindPlanet);

        if(jarfish.SearchOtherPlanet) {
            jarfish.DaysToFindPlanet--;
            GetEvent();
            if(jarfish.DaysToFindPlanet == 0) {
                jarfish.SearchOtherPlanet = false;
                jarfish.DaysToFindPlanet = 0;
                exploreText.text = jarfish.CreatePlanet();
                resIcon.GetComponent<Button>().enabled = true;
                monIcon.GetComponent<Button>().enabled = true;
                logIcon.GetComponent<Button>().enabled = true;
                exploreIcon.GetComponent<Button>().enabled = true;

                exploreWindow.SetActive(true);
                exploreIcon.GetComponent<Button>().enabled = false;

            }
        }

        if(jarfish.StartExploring) {
        jarfish.ExplorationDay--;

            if(jarfish.ExplorationDay == 0) {
                ExplorationLog();
            }
        }

        if(jarfish.IsRepairing) {
            jarfish.repairedPart.elapsedTime--;
            if(jarfish.repairedPart.elapsedTime == 0) {
                jarfish.elapsedTime = 0;
                monitoringWindow.SetActive(true);
                jarfish.repairedPart.Repair();
                jarfish.repairedPart = null;
            }
        }
    }

    private void GetEvent() {
        jarfish.PlaySelectSound();
        string logEvent = jarfish.GetEvent();
        if(!string.IsNullOrEmpty(logEvent)) {
            eventLogText.text = logEvent;
            eventLogWindow.SetActive(true);
            monitoringWindow.SetActive(true);
        }
    }

    private void ExplorationLog() {
        jarfish.PlaySelectSound();
        expLogText.text = "";
        expLogText.text = jarfish.GetResources();
        expLogWindow.SetActive(true);
    }

    public void ExplorationExplorer() {
        jarfish.PlaySelectSound();
        if(jarfish.SearchOtherPlanet)
            return;
        exploreWindow.SetActive(false);
        exploreIcon.GetComponent<Button>().enabled = false;
        explorationDetailWindow.SetActive(true);
    }

    public void ExplorationEngineer() {
        jarfish.PlaySelectSound();
        if(jarfish.SearchOtherPlanet)
            return;
        exploreWindow.SetActive(false);
        exploreIcon.GetComponent<Button>().enabled = false;
        explorationDetailEngineerWindow.SetActive(true);
    }

    public void ExplorationChef() {
        jarfish.PlaySelectSound();
        if(jarfish.SearchOtherPlanet)
            return;
        exploreWindow.SetActive(false);
        exploreIcon.GetComponent<Button>().enabled = false;
        explorationDetailChefWindow.SetActive(true);
    }

    public void StartExploration() {
        jarfish.PlaySelectSound();
        if(jarfish.SearchOtherPlanet)
            return;
        jarfish.StartExploring = true;
        jarfish.water -= jarfish.ExplorerOnAction;
        jarfish.food -= jarfish.ExplorerOnAction * 2;
        jarfish.oxygen -= jarfish.ExplorerOnAction * 3;
        Close("explore detail");
    }

    public void AddExplorer() {
        jarfish.PlaySelectSound();
        peopleText.text = "";
        peopleText.text += " how many people you\n";
        peopleText.text += " wanna send to\n";
        peopleText.text += " explore? " + jarfish.AddExplorerToGo() + " people";
    }

    public void AddEngineer() {
        jarfish.PlaySelectSound();
        engineerPeopleText.text = "";
        engineerPeopleText.text += " how many people you\n";
        engineerPeopleText.text += " wanna send to\n";
        engineerPeopleText.text += " explore? " + jarfish.AddEngingeerToGo() + " people";
    }

    public void AddChef() {
        jarfish.PlaySelectSound();
        chefPeopleText.text = "";
        chefPeopleText.text += " how many people you\n";
        chefPeopleText.text += " wanna send to\n";
        chefPeopleText.text += " explore? " + jarfish.AddChefToGo() + " people";
    }


    public void RepairShip() {
        jarfish.PlaySelectSound();
        if(jarfish.AnyDamage && !jarfish.SearchOtherPlanet) {
            jarfish.GetBrokenPart();
            jarfish.IsRepairing = true;
            jarfish.repairedPart.elapsedTime = jarfish.repairedPart.repairDay;
            repairText.text = " Require " + jarfish.repairedPart.repairDay + " days to repair.";
            monitoringWindow.SetActive(false);
            repairLogWindow.SetActive(true);
        }
    }

    #endregion

}

// TODO:
// Play again with active member explore