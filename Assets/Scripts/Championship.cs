public class Championship {
  private string id;
  private int score;

  private string championshipName;

  private string state;

  public Championship() {}

  public Championship (string idChampionship, int scoreChampionship, string receivedChampionshipName, string stateChampionship)
  {
    id = idChampionship;
    score = scoreChampionship;
    championshipName = receivedChampionshipName;
    state = stateChampionship;
  }

  public string Id
  {
    get { return id; }
    set { id = value; }
  }

  public int Score
  {
    get { return score; }
    set { score = value; }
  }

  public string ChampionshipName
  {
    get { return championshipName; }
    set { championshipName = value; }
  }

  public string State
  {
    get { return state; }
    set { state = value; }
  }
}