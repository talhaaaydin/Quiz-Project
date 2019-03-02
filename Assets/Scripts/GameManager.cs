using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public int correctAnswerPoint = 10;
	public Text questionText, scoreValue;
	public Text[] choices;
	public Question[] questions;
	private List<Question> unAnsweredQuestions;
	private int index;
	private int point;

	// Use this for initialization
	void Start () {
		if (unAnsweredQuestions == null || unAnsweredQuestions.Count == 0 ) {
			unAnsweredQuestions = questions.ToList ();
			point = 0;
		}
		ShowQuestion (Random.Range(0, unAnsweredQuestions.Count));
	}

	void ShowQuestion(int num){
		index = num;
		questionText.text = unAnsweredQuestions [num].question;
		List<int> secilmisler = new List<int>();
		choices [sayiGetir (choices.Length, secilmisler)].text = unAnsweredQuestions [num].choice1;
		choices [sayiGetir (choices.Length, secilmisler)].text = unAnsweredQuestions [num].choice2;
		choices [sayiGetir (choices.Length, secilmisler)].text = unAnsweredQuestions [num].choice3;
		choices [sayiGetir (choices.Length, secilmisler)].text = unAnsweredQuestions [num].correctAnswer;
	}

	int sayiGetir(int range, List<int> secilmisler){
		//Rastgele bir sayı seçiliyor.
		int a = Random.Range (0, range);
		bool boolean = true;
		//Rastgele seçilen sayının daha önce seçilmiş bir sayı olup olmadığı kontrol ediliyor.
		for (int i = 0; i < secilmisler.Count; i++) {
			if (secilmisler [i] == a) {
				//Eğer daha önce seçilmişlerse boolean boolunu false ayarlıyoruz.
				boolean = false;
			}
		}
		//Eğer boolean true ise
		if (boolean) {
			//bu sayıyı seçilmişler listesine ekliyoruz ki bir daha bunu seçmeyelim.
			secilmisler.Add (a);
			//ve sayıyı return ediyoruz.
			return a;
		} else {
			//Eğer boolean false ise yani rastgele seçilen sayı daha önce seçilmiş bir sayı ise bu fonksiyonu tekrar çalıştırıyoruz.
			return sayiGetir (range, secilmisler);
		}
	}

	public void selectChoice(){
		string answer = EventSystem.current.currentSelectedGameObject.GetComponent<Text> ().text;
		string correctAnswer = unAnsweredQuestions [index].correctAnswer;
		//Debug.Log ("user answer: " + answer + ", correct answer: " + correctAnswer);
		if (answer.Equals (correctAnswer)) {
			point++;
			Debug.Log ("Tebrikler doğru cevapladınız");
		} else {
			point = 0;
			Debug.LogWarning ("Yanlış cevap verdiniz, cevabınız " + correctAnswer + " olmalıydı");
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
		unAnsweredQuestions.RemoveAt (index);
		if (unAnsweredQuestions.Count != 0) {
			ShowQuestion (Random.Range (0, unAnsweredQuestions.Count));
		} else if(unAnsweredQuestions.Count == 0 && point>0) {
			Debug.Log ("Tüm soruları doğru cevapladınız!");
			deactivateButtons ();
		}
	}

	void deactivateButtons(){
		for (int i = 0; i < choices.Length; i++) {
			choices [i].GetComponent<Button> ().interactable = false;
		}
	}

	void Update(){
		scoreValue.text = point.ToString ();
	}

}
