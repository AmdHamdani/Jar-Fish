using UnityEngine;
using UnityEngine.UI;

public class JarFish : MonoBehaviour {

    #region variable
    [Header("Member Properties")]
    public Range totalEngineer;
    public Range totalChef;
    public Range totalExplorer;
    
    [Header("Min Resources")]
    public int minFood;
    public int minRawFood;
    public int minWater;
    public int minOxygen;
    [Range(0, 1)]
    public float persenDamage;
    [Range(0, 1)]
    public float persenDeath;
    public SpacePart[] spaceShip;
    public SpacePart[] brokenPart;

    [Header("Plus Resources")]
    public Range rawFoodRange;
    public Range waterRange;
    public int rawToCook;
    [Range(0, 1)]
    public float gemRange;

    [Header("Time")]
    public Range findOtherPlanet;
    public Range explorationRange;
    private float currentTime;
    public int deltaDay = 0;
    public int elapsedTime;

    [Header("Resources")]
    public int food = 100;
    public int water = 100;
    public int rawFood = 0;
    public int oxygen = 100;
    public int damage = 0;
    public int gem = 0;
    public SpacePart repairedPart;

    private string log;
    private bool habitableStat;
    private bool rawFoodStat;
    private bool waterStat;
    private bool oxygenStat;
    private int spaceCapacity;
    private Member member;
    private int explorerToGo;
    private int daysToFindPlanet;
    #endregion

    void Start() {
        CreateMember();
        // Debug.Log(member.explorer);
    }
    
    public void PlaySelectSound() {
        GetComponent<AudioSource>().Play();
    }

    public void CreateMember() {
        member.engineer = Randomer(totalEngineer);
        member.chef = Randomer(totalChef);
        member.explorer = Randomer(totalExplorer);
        spaceCapacity = member.engineer + member.chef + member.explorer;
    }

    private int Randomer(Range range) {
        return Random.Range(range.min, range.max);
    }

    private void EndDay() {
        deltaDay++;
        food -= spaceCapacity * minFood;

        if(food <= 0)
            food = 0;

        water -= spaceCapacity * minWater;

        if(water <= 0)
            water = 0;
        
        oxygen -= spaceCapacity;
        if(oxygen <= 0)
            oxygen = 0;


        ProduceFood = member.chef * rawToCook;
        if(rawFood != 0) {
            food += ProduceFood;
            rawFood -= rawToCook;
        }
        if(rawFood <= 0)
            rawFood= 0;
    }

    private string Log() {
        string log = "";
        log += " - Food is " + food + " kg\n";
        log += " - Raw food is " + rawFood + " kg\n";
        log += " - Water is " + water + " l\n";
        log += " - O2 is " + oxygen + " l\n";
        log += " - Damage on ship " + damage + "\n";
        log += " - Gem is " + gem + " pieces\n";
        log += " - Produce " + ProduceFood + " kg\n";
        return log;
    }

    public string Logger() {
        EndDay();
        log = "";
        log += " Day " + deltaDay + "\n";
        log += Log();
        return log;
    }

    public string ResourceLog() {
        return Log();
    }

    public void SearchPlanet() {
        DaysToFindPlanet = Randomer(findOtherPlanet);
        SearchOtherPlanet = true;
    }

    public string CreatePlanet() {
        habitableStat = Random.Range(0.0f, 1f) < .5f ? true : false;
        rawFoodStat = Random.Range(0.0f, 1f) < .5f ? true : false;
        waterStat = Random.Range(0.0f, 1f) < .5f ? true : false;
        oxygenStat = Random.Range(0.0f, 1f) < .5f ? true : false;

        ExplorationDay = Randomer(explorationRange);

        string str = "";
        str += " Habitable : " + habitableStat + "\n";
        str += " Raw Food  : " + rawFoodStat + "\n";
        str += " Water     : " + waterStat + "\n";
        str += " Oxygen    : " + oxygenStat + "\n";
        str += " Exploration " + ExplorationDay + " days.";
        return str;
    }

    public int AddExplorerToGo() {
        explorerToGo = 0;
        if(explorerToGo <= member.explorer & member.explorer != 0) {
            explorerToGo++;
            member.explorer--;
        }
        return explorerToGo;
    }

    public int AddEngingeerToGo() {
        explorerToGo = 0;
        if(explorerToGo <= member.engineer && member.engineer != 0) {
            explorerToGo++;
            member.engineer--;
        }
        return explorerToGo;
    }

    public int AddChefToGo() {
        explorerToGo = 0;
        if(explorerToGo <= member.chef && member.chef != 0) {
            explorerToGo++;
            member.chef--;
        }
        return explorerToGo;
    }

    public string GetResources() {
        string str = "";

        if(Random.Range(0f, 1f) < persenDeath) {
            explorerToGo--;
        }

        member.explorer = explorerToGo;

        int tempRawFood = Randomer(rawFoodRange) * explorerToGo;
        int tempWater = Randomer(waterRange) * explorerToGo;
        int tempGem = (Random.Range(0f, 1f) < gemRange) ? 1 * explorerToGo : 0;

        str += " An explorer had\n";
        str += " died.\n";
        str += " They found\n";

        if(rawFoodStat)
            str += " Raw Food  : " + tempRawFood + " kg\n";
        if(waterStat)
            str += " Water     : " + tempWater + " l\n";
        if(oxygenStat) {
            oxygen = 100;
            str += " Oxygen    : " + 100 + " l\n";
        }

        rawFood += tempRawFood;
        water += tempWater;
        gem += tempGem;

        return str;
    }

    public string GetEvent() {
        string str = "";
        if(Random.Range(0f, 1f) < persenDamage) {
            int size = Random.Range(1, 3);
            SpacePart[] temp = new SpacePart[size];

            for(int i = 0; i < size; i++) {
                // temp[i] = spaceShip[Random.Range(0, spaceShip.Length)];
                int index = Random.Range(0, spaceShip.Length);
                spaceShip[index].DoDamage();
            }

            str = " SpaceShip hit by\n space junk . . .";
        }

        return str;
    }
    
    public void GetBrokenPart() {
        foreach(SpacePart part in spaceShip) {
            if(part.isDamage) {
                repairedPart = part;
                return;
            }
        }
    }

    #region properties
    public bool StartExploring { get; set; }
    public int ExplorationDay { get; set; }

    public int ExplorerOnAction {
        get {
            return explorerToGo;
        }
    }

    public string Engineer {
        get {
            return "Engineer : " + member.engineer + " people";
        }        
    }

    public string Chef {
        get {
            return "Chef     : " + member.chef + " people";
        }
    }

    public string Explorer {
        get {
            return "Explorer : " + member.explorer + " people";
        }
    }

    public int DaysToFindPlanet { get; set; }
    public bool SearchOtherPlanet { get; set; }
    public bool IsRepairing { get; set; }

    public bool IsSpaceshipBroken {
        get {
            int i = 0;
            foreach(SpacePart part in spaceShip) {
                if(part.isDamage)
                    i++;
            }

            if(i == spaceShip.Length) {
                return true;
            }
            return false;
        }
    }

    public bool AnyDamage {
        get {
            foreach(SpacePart part in spaceShip) {
                if(part.isDamage)
                    return true;
            }
            return false;
        }
    }

    public bool StillAlive {
        get {
            if((food == 0 && water == 0) || oxygen == 0)
                return true;
            return false;
        }
    }

    public int ProduceFood { get; set; }
    #endregion
}

#region data
[System.Serializable]
public struct Range {
    public int min;
    public int max;
}

public struct Member {
    public int engineer;
    public int chef;
    public int explorer;
}

public struct Item {
    public int food;
    public int water;
    public int rawFood;
}

[System.Serializable]
public class SpacePart {
    public Image part;
    public Sprite damage;
    public Sprite normal;
    public bool isDamage;
    public int repairDay;
    public int elapsedTime;

    public void DoDamage() {
        part.sprite = damage;
        isDamage = true;
    }

    public void Repair() {
        part.sprite = normal;
        isDamage = false;
    }
}
#endregion