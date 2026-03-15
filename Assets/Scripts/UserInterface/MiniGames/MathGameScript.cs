using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Reflection;

public class MathGameScript : MonoBehaviour
{
    #region Callbacks
    private void Start() => InitializeGame();

    private void Update()
    {
        HandleAudioFeedback();
        HandleInput();

        if (problem > problemcap)
        {
            HandleGameEnd();
        }
        if (impossibleMode)
        {
            num = new float[]
		    {
			    (int)UnityEngine.Random.Range(0, 9999f),
                (int)UnityEngine.Random.Range(0, 9999f),
                (int)UnityEngine.Random.Range(0, 9999f)
		    };
            string baseQuestion = sign == 0 ? $"{num[0]} + ({num[1]} × {num[2]} = ?" : $"({num[0]} ÷ {num[1]}) + {num[2]} =?";
            questionText.text = $"Solve Math Q{problem}: \n" + ApplyGlitchEffect(baseQuestion);
            questionText2.text = "\n" + ApplyGlitchEffect(baseQuestion);
            questionText3.text = "\n" + ApplyGlitchEffect(baseQuestion);
        }
    }
    #endregion

    #region Initialization
    private void InitializeGame()
    {
        lowBudgetAudioManagementShit lbams = lowBudgetAudioManagementShit.Instance;
        string dific = PlayerPrefs.GetString("CurDifficulity", "normal");
        problemcap = dific == "easy" ? 3 : dific == "normal" ? 6 : dific == "hard" ? 9 : dific == "expert" || dific == "maniac" ? 12 : 3;
        int problemCapAlt = dific == "easy" ? 9 : dific == "normal" ? 6 : dific == "hard" ? 3 : dific == "expert" || dific == "maniac" ? 0 : 6;
        
        specialCodes = new Dictionary<string, Action>
        {
            { "31718", () => 
                { 
                    StartCoroutine(CheatText("THIS IS WHERE IT ALL BEGAN")); SceneManager.LoadSceneAsync("TestRoom"); 
                } 
            }
        };
        ChallengeCodes = new Dictionary<string, Action>
        {
            { "92.28.211.234", () => 
                { 
                    gc.ExclusiveRoute = "SecretEndChal";
                    questionText.text = ".......did you really tryna dox me bro?";
                    padChallengeCode = true;
                }
            }
        };
        tbstransis.Rebind();
        tbstransis.Play("yooo", -1, 0f);

        baldiAudio.SetIgnoreListenerPause(true);
        lg.learnMusic.SetIgnoreListenerPause(true);
        if (!gc.spoopMode && gc.mode != "zerullclassic") lbams.SchoolMusic.SetIgnoreListenerPause(true);
        for (int i = 0; i < questionBlockerSlots.Length; ++i) questionBlockerSlots[i].SetActive(false);
        for (int i = 0; i < problemCapAlt; ++i) questionBlockerSlots[i].SetActive(true);

        endDelay = gc.spoopMode ? Delay : DelayPreSpoop;
        lg.ActivateLearningGame();
        baldiFeed.enabled = false;
        Baldtalk.SetActive(false);
        StaticBG.SetActive(false);
        ZerullFeed.SetActive(false);
        ChairFeed.SetActive(false);

        if (gc.notebooks == 1 && gc.mode != "zerullclassic")
        {
            baldiAudio.QueueAudio(bal_intro);
            //QueueAudio(bal_howto);
        }

        if (gc.spoopMode && gc.mode != "zerullclassic")
        {
            BlackCoverUp.SetActive(true);
            baldiFeedTransform.gameObject.SetActive(false);
            baldiFeedI.enabled = false;
            Baldtalk.SetActive(true);
        }
        if (gc.mode == "zerullclassic")
        {
            if (gc.notebooks > 2) allanswerWrongInt = UnityEngine.Random.Range(0,2);
            bool chair = PlayerPrefsExtension.GetBool("BeatedUpZerull");
            ProvideHintOrFeedback(allanswerWrongInt);
            Baldtalk.SetActive(false);
            BlackCoverUp.SetActive(false);
			lg.learnMusic.ClearQueue(true);
			baldiAudio.ClearQueue(true);
			playerAnswer.gameObject.SetActive(false);
			baldiFeedTransform.gameObject.SetActive(false);
			baldiFeed.enabled = false;
			StaticBG.SetActive(true);
            if (!chair) ZerullFeed.SetActive(true);
            if (chair) ChairFeed.SetActive(true);
		}

        if (gc.mode != "zerullclassic") NewProblem();
        
    }
    #endregion

    #region Input Handling
    private void HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && questionInProgress && gc.mode != "zerullclassic")
        {
            questionInProgress = false;
            CheckAnswer();
        }
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && gc.mode == "zerullclassic")
        {
            GC.Collect();
            ExitGame();
            lg.DeactivateLearningGame(gameObject,allanswerWrongInt);
        }
                
    }
    #endregion

    #region Audio Feedback
    private void HandleAudioFeedback()
    {
        if (baldiAudio.audioDevice.isPlaying) UpdateAudioFeedback();
        else baldiFeedI.sprite = talkSprites[0];
    }

    private void UpdateAudioFeedback()
    {
        baldiAudio.audioDevice.GetOutputData(clipSampleData, 0);
        float clipLoudness = 0f;

        foreach (float sample in clipSampleData) clipLoudness += Mathf.Abs(sample) * baldiAudio.audioDevice.volume;

        int spriteIndex = Mathf.RoundToInt(Mathf.Clamp(clipLoudness * 2f, 0f, 6f));
        baldiFeedI.sprite = talkSprites[spriteIndex];
    }
    #endregion

    #region Problem Generation
    private void NewProblem()
    {
        ResetProblemUI();

        if (problem <= problemcap) GenerateMathProblem();
        else  HandleProblemCompletion();
    }

    private void ResetProblemUI()
    {
        playerAnswer.text = string.Empty;
        problem++;
        playerAnswer.ActivateInputField();
        questionInProgress = true;
    }

    private void GenerateMathProblem()
    {
        if (!gc.spoopMode || gc.mode != "zerullclassic") StartCoroutine(PlayClassicMusic());
        if (gc.notebooks == 2 && problem == problemcap) baldiAudio.QueueAudio(scaryproblem);
        if (gc.notebooks <= 2 && problem != problemcap) baldiAudio.QueueAudio(bal_problems[problem - 1]);

        if ((gc.mode == "endless" && gc.notebooks == 2 && problem == problemcap && !impossibleQuestionShown) || (gc.mode == "story" && gc.notebooks > 1 && problem == problemcap))
        {
            GenerateImpossibleProblem();
            impossibleQuestionShown = true;
        }
        else
        {
            if (!gc.PadSEToggle) GenerateMathProblemMain();
            else GenerateImpossibleProblem();
        }
    }
    private void CreateQuestion(string Difficulity = "easy") // what the actual fuck
    {
        int randomRangeInt = Difficulity == "easy" ? 10 : Difficulity == "normal" ? 50 : Difficulity == "hard" ? 100 : Difficulity == "expert" ? 250 : Difficulity == "maniac" ? 1000 : 10;
        int MultiplicationDifficulity = Difficulity == "easy" ? 0 : Difficulity == "normal" ? 1 : Difficulity == "hard" ? 2 : Difficulity == "expert" ? 3 : Difficulity == "maniac" ? 4 : 1;
        int[] ReduceArray = new int[]
		{
			1,
			2,
			3,
			4,
			5,
            6,
            7,
            8,
            9,
            10
		};
        // math shits
        num = new float[]
		{
			(int)UnityEngine.Random.Range(0, (float)randomRangeInt + 1),
            (int)UnityEngine.Random.Range(0, (float)randomRangeInt + 1),
            (int)UnityEngine.Random.Range(0, (float)randomRangeInt + 1),
            (int)UnityEngine.Random.Range(0, (float)randomRangeInt + 1),
            (int)UnityEngine.Random.Range(0, (float)randomRangeInt + 1)
		};
        float[] numMult = new float[]
		{
			(int)UnityEngine.Random.Range(0, 11),
            (int)UnityEngine.Random.Range(0, 26),
            (int)UnityEngine.Random.Range(0, 101),
            (int)UnityEngine.Random.Range(0, 251),
            (int)UnityEngine.Random.Range(0, 1001)
		};
        float[] numMult1 = new float[]
		{
			(int)UnityEngine.Random.Range(0, 11),
            (int)UnityEngine.Random.Range(0, 26),
            (int)UnityEngine.Random.Range(0, 101),
            (int)UnityEngine.Random.Range(0, 251),
            (int)UnityEngine.Random.Range(0, 1001)
		};
        float[] numMult2 = new float[]
		{
			(int)UnityEngine.Random.Range(0, 11),
            (int)UnityEngine.Random.Range(0, 26),
            (int)UnityEngine.Random.Range(0, 101),
            (int)UnityEngine.Random.Range(0, 251),
            (int)UnityEngine.Random.Range(0, 1001)
		};
        //calculating logic
        int wuh = ReduceArray[UnityEngine.Random.Range(0,10)],wuh2 = (int)Math.Pow(2.0, wuh);
        float plus = num[0] + num[1];
        float plusplus = num[0] + num[1] + num[2];
        float plusplusplus = num[0] + num[1] + num[2] + num[3];
        float plusplusplusplus = num[0] + num[1] + num[2] + num[3] + num[4];
        float minus = num[0] - num[1];
        float minusminus = num[0] - num[1] - num[2];
        float minusminusminus = num[0] - num[1] - num[2] - num[3];
        float minusminusminusminus = num[0] - num[1] - num[2] - num[3] - num[4];
        float plusminus = num[0] + num[1] - num[2];
        float minusplus = num[0] - num[1] + num[2];
        float plusplusminus = num[0] + num[1] + num[2] - num[3];
        float plusminusplus = num[0] + num[1] - num[2] + num[3];
        float minusplusplus = num[0] - num[1] + num[2] + num[3];
        float minusminusplus = num[0] - num[1] - num[2] + num[3];
        float minusplusminus = num[0] - num[1] + num[2] - num[3];
        float plusplusplusminus = num[0] + num[1] + num[2] + num[3] - num[4]; // holy variable spam
        float plusplusminusplus = num[0] + num[1] + num[2] - num[3] + num[4];
        float plusminusplusplus = num[0] + num[1] - num[2] + num[3] + num[4];
        float minusplusplusplus = num[0] - num[1] + num[2] + num[3] + num[4];
        float plusminusminusminus = num[0] + num[1] - num[2] - num[3] - num[4];
        float minusplusminusminus = num[0] - num[1] + num[2] - num[3] - num[4];
        float minusminusplusminus = num[0] - num[1] - num[2] + num[3] - num[4];
        float minusminusminusplus = num[0] - num[1] - num[2] - num[3] + num[4];
        float minusplusminusplus = num[0] - num[1] + num[2] - num[3] + num[4];
        float plusminusplusminus = num[0] + num[1] - num[2] + num[3] - num[4];
        float plusplusminusminus = num[0] + num[1] + num[2] - num[3] - num[4];
        float minusminusplusplus = num[0] - num[1] - num[2] + num[3] + num[4];
        float minusplusplusminus = num[0] - num[1] + num[2] + num[3] - num[4];
        float plusminusminusplus = num[0] + num[1] - num[2] - num[3] + num[4];
        double mult = numMult[MultiplicationDifficulity] * numMult1[MultiplicationDifficulity];
        double multplus = numMult[MultiplicationDifficulity] * (num[1]+num[2]);
        double multminus = numMult[MultiplicationDifficulity] * (num[1]-num[2]);
        double multmult = numMult[MultiplicationDifficulity] * numMult1[MultiplicationDifficulity] * numMult2[MultiplicationDifficulity];
        int dividedIntNoRound = (int)num[0] >> wuh;
        double powerOfZero = Math.Pow(num[0], 0.0f);
        double square = Math.Pow(num[0], 2.0f);
        double cube = Math.Pow(num[0], 3.0f);
        double quart = Math.Pow(num[0], 4.0f);
        //string text logic
        string plustext = $"{num[0]} + {num[1]}";
        string plusplustext = $"{num[0]} + {num[1]} + {num[2]}";
        string plusplusplustext = $"{num[0]} + {num[1]} + {num[2]} + {num[3]}";
        string plusplusplusplustext = $"{num[0]} + {num[1]} + {num[2]} + {num[3]} + {num[4]}";
        string minustext = $"{num[0]} - {num[1]}";
        string minusminustext = $"{num[0]} - {num[1]} - {num[2]}";
        string minusminusminustext = $"{num[0]} - {num[1]} - {num[2]} - {num[3]}";
        string minusminusminusminustext = $"{num[0]} - {num[1]} - {num[2]} - {num[3]} - {num[4]}";
        string plusminustext = $"{num[0]} + {num[1]} - {num[2]}";
        string minusplustext = $"{num[0]} - {num[1]} + {num[2]}";
        string plusplusminustext = $"{num[0]} + {num[1]} + {num[2]} - {num[3]}";
        string plusminusplustext = $"{num[0]} + {num[1]} - {num[2]} + {num[3]}";
        string minusplusplustext = $"{num[0]} - {num[1]} + {num[2]} + {num[3]}";
        string minusminusplustext = $"{num[0]} - {num[1]} - {num[2]} + {num[3]}";
        string minusplusminustext = $"{num[0]} - {num[1]} + {num[2]} - {num[3]}";
        string plusplusplusminustext = $"{num[0]} + {num[1]} + {num[2]} + {num[3]} - {num[4]}";
        string plusplusminusplustext = $"{num[0]} + {num[1]} + {num[2]} - {num[3]} + {num[4]}";
        string plusminusplusplustext = $"{num[0]} + {num[1]} - {num[2]} + {num[3]} + {num[4]}";
        string minusplusplusplustext = $"{num[0]} - {num[1]} + {num[2]} + {num[3]} + {num[4]}";
        string plusminusminusminustext = $"{num[0]} + {num[1]} - {num[2]} - {num[3]} - {num[4]}";
        string minusplusminusminustext = $"{num[0]} - {num[1]} + {num[2]} - {num[3]} - {num[4]}";
        string minusminusplusminustext = $"{num[0]} - {num[1]} - {num[2]} + {num[3]} - {num[4]}";
        string minusminusminusplustext = $"{num[0]} - {num[1]} - {num[2]} - {num[3]} + {num[4]}";
        string minusplusminusplustext = $"{num[0]} - {num[1]} + {num[2]} - {num[3]} + {num[4]}";
        string plusminusplusminustext = $"{num[0]} + {num[1]} - {num[2]} + {num[3]} - {num[4]}";
        string plusplusminusminustext = $"{num[0]} + {num[1]} + {num[2]} - {num[3]} - {num[4]}";
        string minusminusplusplustext = $"{num[0]} - {num[1]} - {num[2]} + {num[3]} + {num[4]}";
        string minusplusplusminustext = $"{num[0]} - {num[1]} + {num[2]} + {num[3]} - {num[4]}";
        string plusminusminusplustext = $"{num[0]} + {num[1]} - {num[2]} - {num[3]} + {num[4]}";
        string multtext = $"{numMult[MultiplicationDifficulity]} X {numMult1[MultiplicationDifficulity]}";
        string multplustext = $"{numMult[MultiplicationDifficulity]} X ({num[1]} + {num[2]})";
        string multminustext = $"{numMult[MultiplicationDifficulity]} X ({num[1]} - {num[2]})";
        string multmulttext = $"{numMult[MultiplicationDifficulity]} X {numMult1[MultiplicationDifficulity]} X {numMult2[MultiplicationDifficulity]}";
        string dividedIntNoRoundtext = $"{num[0]} / {wuh2} (no roundding stuff,interger type division)";
        string powerOfZerotext = $"{num[0]}<sup>{0f}</sup>";
        string squaretext = $"{num[0]}<sup>{2f}</sup>";
        string cubetext = $"{num[0]}<sup>{3f}</sup>";
        string quarttext = $"{num[0]}<sup>{4f}</sup>";

        double[] MathType = new double[] //killme
        {
            plus,
            plusplus,
            plusplusplus,
            plusplusplusplus,
            minus,
            minusminus,
            minusminusminus,
            minusminusminusminus,
            plusminus,
            minusplus,
            plusplusminus,
            plusminusplus,
            minusplusplus,
            minusminusplus,
            minusplusminus,
            plusplusplusminus,
            minusplusminusminus,
            plusminusplusplus,
            minusplusplusplus,
            plusminusminusminus,
            minusplusminusminus,
            minusminusplusminus,
            minusminusminusplus,
            minusplusminusplus,
            plusminusplusminus,
            plusplusminusminus,
            minusminusplusplus,
            minusplusplusminus,
            plusminusminusplus,
            mult,
            multplus,
            multminus,
            multmult,
            dividedIntNoRound,
            powerOfZero,
            square,
            cube,
            quart
        };
        string[] probleTex = new string[]
        {
            plustext,
            plusplustext,
            plusplusplustext,
            plusplusplusplustext,
            minustext,
            minusminustext,
            minusminusminustext,
            minusminusminusminustext,
            plusminustext,
            minusplustext,
            plusplusminustext,
            plusminusplustext,
            minusplusplustext,
            minusminusplustext,
            minusplusminustext,
            plusplusplusminustext,
            minusplusminusminustext,
            plusminusplusplustext,
            minusplusplusplustext,
            plusminusminusminustext,
            minusplusminusminustext,
            minusminusplusminustext,
            minusminusminusplustext,
            minusplusminusplustext,
            plusminusplusminustext,
            plusplusminusminustext,
            minusminusplusplustext,
            minusplusplusminustext,
            plusminusminusplustext,
            multtext,
            multplustext,
            multminustext,
            multmulttext,
            dividedIntNoRoundtext,
            powerOfZerotext,
            squaretext,
            cubetext,
            quarttext
        };
        string problemTextThing = "";
        int[] QuestionTypesInt = Difficulity == "easy" ? new int[] {0,1,4,5,8,9,29,30,31} : Difficulity == "normal" ? new int[] {0,1,2,4,5,6,7,8,29,30,21,31,34} : new int[] {0,1,3,4};
        int randomrangeThing = QuestionTypesInt[UnityEngine.Random.Range(0,QuestionTypesInt.Length)];
        solution = MathType[randomrangeThing].ToString();
        problemTextThing = probleTex[randomrangeThing];
        questionText.text = $"Solve Math Q{problem}: \n{problemTextThing} = ?";
    }

    private void GenerateMathProblemMain()
    {
        CreateQuestion(PlayerPrefs.GetString("CurDifficulity", "normal"));
        
        double dividedRoundUpNumb = Math.Round(num[0] / num[1],2);

        //QueueAudio(bal_numbers[Mathf.RoundToInt(num[0])]);
        //solution = sign == 0 ? num[0] + num[1] : sign == 1 ? num[0] - num[1] : sign == 2 ? num[0] * num[1] : (float)dividedRoundUpNumb;
        //string RoundUpText = sign == 3 ? " (Then round up the number to 2 digits)": "";
        //string signText = sign == 0 ? "+" : sign == 1 ? "-": sign == 2 ? "x": "/";
        //questionText.text = $"Solve Math Q{problem}: \n{num[0]}{signText}{num[1]}{RoundUpText} = ?";
        //QueueAudio(sign == 0 ? bal_plus : bal_minus);
        //QueueAudio(bal_numbers[Mathf.RoundToInt(num[1])]);
        //QueueAudio(bal_equals);
    }

    private void GenerateImpossibleProblem()
    {
        impossibleMode = true;
        sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
        baldiAudio.QueueAudio(bal_screech);
        //QueueAudio(bal_times);
        baldiAudio.QueueAudio(bal_screech);
        //QueueAudio(bal_divided);
        baldiAudio.QueueAudio(bal_screech);
        //QueueAudio(bal_equals);
    }

    private string ApplyGlitchEffect(string text)
    {
        string[] glitchChars = { "!", "@", "#", "$", "%", "^", "&", "*", "?", "~" };
        System.Text.StringBuilder glitchyText = new System.Text.StringBuilder();

        foreach (char c in text)
        {
            if (UnityEngine.Random.value > 0.8f)  glitchyText.Append(glitchChars[UnityEngine.Random.Range(0, glitchChars.Length)]);
            else glitchyText.Append(c);
        }

        return glitchyText.ToString();
    }
    #endregion

    #region Answer Evaluation
    public void OKButton() => CheckAnswer();

    public void CheckAnswer()
    {
        if (CheckSpecialCodes(playerAnswer.text)) return;

        if (gc.mode != "zerullclassic")
		{
			if (problem <= problemcap)
			{
				if (IsCorrectAnswer()) HandleCorrectAnswer();
				else HandleIncorrectAnswer();
			}
		}
		else
		{
			GC.Collect();
            ExitGame();
            lg.DeactivateLearningGame(gameObject,allanswerWrongInt);
		}

        ResetInputState();
    }

    private bool CheckSpecialCodes(string answer)
    {
        if (specialCodes.TryGetValue(answer, out Action action))
        {
            action.Invoke();
            return false;
        }

        return false;
    }
    private bool CheckChallengeCodes(string answer)
    {
        if (ChallengeCodes.TryGetValue(answer, out Action action))
        {
            action.Invoke();
            return false;
        }

        return false;
    }

    private bool IsCorrectAnswer()
    {
        return playerAnswer.text == solution.ToString() && !impossibleMode;
    }

    private void HandleCorrectAnswer()
    {
        results[problem - 1].sprite = correct;
        baldiAudio.ClearQueue(true);
        int praiseIndex = UnityEngine.Random.Range(0, bal_praises.Length);
        baldiAudio.QueueAudio(bal_praises[praiseIndex]);
        NewProblem();
        scoreSystemManager.Instance.AddScore(75);
    }

    private void HandleIncorrectAnswer()
    {
        if (CheckChallengeCodes(playerAnswer.text) && problemsWrong == (problemcap-1)) return;
        problemsWrong++;
        results[problem - 1].sprite = incorrect;

        if (!gc.spoopMode)
        {
            //baldiFeedI.enabled = false;
            //baldiFeed.enabled = true;
            //baldiFeed.SetTrigger("angry");
            gc.ActivateSpoopMode();
        }
        scoreSystemManager.Instance.AddScore(275);
        HandleBaldiAnger();
        baldiAudio.ClearQueue(true);
        if (impossibleQuestionShown) impossibleMode = false;
        NewProblem();
    }

    private void HandleBaldiAnger()
    {
        if (problem == problemcap) Singleton<OtherMainStuffManager>.Instance.AngerShit(0.2f*lg.angerMult, 0f,false, "all");
        else  Singleton<OtherMainStuffManager>.Instance.AngerShit(0f, 0.15f*lg.tempAngerMult,true, "all");
    }

    private void ResetInputState()
    {
        minusButton.interactable = true;
        context = string.Empty;
        negative = false;
    }
    #endregion

    #region Game End Handling
    private void HandleProblemCompletion()
    {
        if (!gc.spoopMode) questionText.text = "WOW! YOU EXIST!";
        else  ProvideHintOrFeedback();
    }
    
    private void ProvideHintOrFeedback(int allanswerWrongInt = 0)
    {
        if (gc.mode == "endless" && problemsWrong <= 0) questionText.text = endlessHintText[UnityEngine.Random.Range(0, endlessHintText.Length)];
        if (problemsWrong >= problemcap)
        {
            lowBudgetAudioManagementShit lbams = lowBudgetAudioManagementShit.Instance;
            Singleton<OtherMainStuffManager>.Instance.HearingShit(7f, null, playerPosition, "all", true);
            lbams.PlayClip(lbams.MainSource1, lbams.deadbel);
        }
        if (gc.mode == "zerullclassic" && problemsWrong <= 0)
        {
            bool chairr = PlayerPrefsExtension.GetBool("BeatedUpZerull");
            if (!chairr)
            {
                int index = UnityEngine.Random.Range(0, zerullQuotes.Length);
                if (gc.notebooks > 2 && allanswerWrongInt != 1) questionText.text = zerullQuotes[index];
                if (gc.notebooks > 2 && allanswerWrongInt == 1) questionText.text = "bro.." + '\n' + "youre so fucking cooked LMFAO";
                if (gc.notebooks == 2) questionText.text = "Jeezpers, you really want to suffer huh?"+ '\n'+ "fine by me i suppose";
                if (gc.notebooks == 1) questionText.text = "Huh, why did i got teleported into this dimension?"+ '\n'+ "oh its you again, the fuck do you want";
            }
            if (chairr) questionText.text = "chair";
            questionText2.text = questionText3.text = string.Empty;
            if (allanswerWrongInt == 1) for (int i = 0; i < problemcap; ++i) results[i].sprite = incorrect;
            else for (int i = 0; i < problemcap; ++i) results[i].sprite = correct;
			return;
		}
            questionText.text = hintText[UnityEngine.Random.Range(0, hintText.Length)];
        questionText2.text = questionText3.text = string.Empty;
        if (gc.mode == "story" && problemsWrong >= problemcap)
        {
            gc.failedNotebooks++;
            if (gc.failedNotebooks < gc.maxNotebooks && gc.PadSEToggle)
            {
                questionText.text = "Keep Doing TS shit my guy, " + gc.failedNotebooks + "/" + gc.maxNotebooks + " left";
                questionText2.text = questionText3.text = string.Empty;
            }
            if (gc.failedNotebooks == 1 && gc.notebooks < gc.UnlockAmount)
            {
                endDelay = jer_SecretAAW.audClip.length;
                baldiAudio.PlaySingleClip(jer_SecretAAW);
                if (!padChallengeCode)
                {
                    if (gc.SlotsAmmount >= 5)
                    {
                        questionText.text = "fuck you, 2 of your slots will be gone";
                        Singleton<OtherMainStuffManager>.Instance.ChangeItemSlot(GameControllerScript.Instance.SlotsAmmount-2,true);
                    }
                    else if (gc.SlotsAmmount <= 5) questionText.text = "fuck you, 2 of your slots will be go- oh wait you have under 5 slots my bad gang";
                }
                else if (padChallengeCode)
                {
                    string CurrentCharName = PlayerPrefs.GetString("CurrentCharacter", "");
                    if (CurrentCharName == "ClassicPlayer") 
                    {
                        gc.ExclusiveRoute = "ClassicPlayerSecretEndChal";
                        questionText.text = ".......did you really tryna dox me bro?.... wait no way youre trying it as that weak ass playables character, yknow what? lets gave you the suffering you wanted";
                        Singleton<OtherMainStuffManager>.Instance.HighSchoolDropOut();
                        gc.SlotsAmmount = 1;
                        Singleton<OtherMainStuffManager>.Instance.slot();
                    }
                }
                questionText2.text = questionText3.text = string.Empty;
                gc.PadSEToggle = true;
            }
            if (gc.failedNotebooks == gc.maxNotebooks)
            {
                questionText.text = "ok, time for the finale, betch";
                questionText2.text = string.Empty;
                questionText3.text = string.Empty;
            }
        }
    }

    private void HandleGameEnd()
    {
        endDelay -= Time.unscaledDeltaTime;
        if (endDelay <= 0f)
        {
            lg.learnMusic.SetIgnoreListenerPause(false);
            lg.learnMusic.ClearQueue(true);
            GC.Collect();
            ExitGame();
        }
    }

    private void ExitGame()
    {
        if (problemsWrong <= 0 && gc.mode == "endless")
        {
            gc.baldiScrpt.GetAngry(-1f);
            gc.famishScrpt.GetAngry(-1f);
        }

        lg.DeactivateLearningGame(gameObject);
    }
    #endregion

    #region Utility Methods
    private IEnumerator CheatText(string text)
    {
        while (true)
        {
            questionText.text = text;
            questionText2.text = string.Empty;
            questionText3.text = string.Empty;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator PlayClassicMusic()
    {
        int musicIndex = problem - 1;
        if (musicIndex < 0 || musicIndex >= learnMusics.Length) yield break;

        if (musicIndex >= 1)
        {
            lg.learnMusic.SetLoop(false);
            yield return new WaitWhile(() => lg.learnMusic.audioDevice.isPlaying);
        }

        if (!gc.spoopMode)
        {
            lg.learnMusic.ClearQueue(true);
            lg.learnMusic.QueueAudio(learnMusics[musicIndex]);
            lg.learnMusic.SetLoop(true);
        }
        else lg.learnMusic.ClearQueue(true);
    }

    public void ButtonPress(int value)
    {
        if (value >= 0 && value <= problemcap)
        {
            context += value;
            playerAnswer.text = context;
        }
        else if (value == -1)
        {
            negative = !negative;
            playerAnswer.text = negative ? "-" + context : context;
            context = playerAnswer.text;
            minusButton.interactable = !negative;
        }
        else
        {
            playerAnswer.text = context = string.Empty;
            negative = false;
            minusButton.interactable = true;
        }
    }
    #endregion

    #region Serialized Fields
    [Header("Game References")]
    public GameControllerScript gc;
    public Animator tbstransis,DitherTransis;
    public LearningGameManager lg;
    public booksInteract nbScri;
    public Vector3 playerPosition;

    [Header("UI Elements")]
    [SerializeField] private Image[] results = new Image[3];
    [SerializeField] private GameObject[] questionBlockerSlots = new GameObject[3];
    [SerializeField] private TMP_InputField playerAnswer;
    [SerializeField] private TMP_Text questionText, questionText2, questionText3;
    [SerializeField] private Animator baldiFeed;
    [SerializeField] private Transform baldiFeedTransform;
    [SerializeField] private GameObject BlackCoverUp;
    [SerializeField] private Button minusButton;
    [SerializeField] private Image baldiFeedI;
    [SerializeField] private GameObject Baldtalk;
    [Header("Zerull UI Elements")]
    [SerializeField] private GameObject ZerullFeed,ChairFeed;
    [SerializeField] private GameObject StaticBG;
    public string[] zerullQuotes;

    [Header("Audio Clips")]
    [SerializeField] private AudioManagerLiveReaction baldiAudio;
    [SerializeField] private AudioObjectyeah bal_intro, bal_howto, bal_plus, bal_minus, bal_times, bal_divided, bal_equals, bal_screech,jer_SecretAAW,scaryproblem;
    [SerializeField] private AudioObjectyeah[] bal_numbers, bal_praises, bal_problems, learnMusics;

    [Header("Sprites")]
    [SerializeField] private Sprite[] talkSprites;
    [SerializeField] private Sprite correct, incorrect;
    #endregion

    #region Internal State
    [Header("Game State")]
    public string context = string.Empty,solution= string.Empty;
    public float[] num;
    public int problemcap = 9;
    public bool questionInProgress, impossibleMode, negative,thepadgotaawed;

    [SerializeField] private bool impossibleQuestionShown,padChallengeCode;
    private const int SampleDataLength = 64;
    private string[] hintText = { "I GET ANGRIER FOR EVERY PROBLEM YOU GET WRONG", "I HEAR EVERY DOOR YOU OPEN" }, endlessHintText = { "That's more like it...", "Keep up the good work or see me after class..." };
    private float endDelay;
    private int problem, audioInQueue, problemsWrong, sign,allanswerWrongInt;
    public float DelayPreSpoop = 4f, Delay = 0.5f;
    private float[] clipSampleData = new float[SampleDataLength];
    private Dictionary<string, Action> specialCodes,ChallengeCodes;
    #endregion
}