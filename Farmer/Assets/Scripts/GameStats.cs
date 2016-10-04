using UnityEngine;
using System.Collections;

public class GameStats : MonoBehaviour {

	private int _level = 1;
	public int _requiredExperience = 100;
	public int _currentExperience = 0;
	
	public void AddExperience(int experience)
	{
        while (experience > 0)
        {
            if (_currentExperience + experience >= _requiredExperience)
            {
                _level++;
                _requiredExperience *= 5;
                _currentExperience = 0;

                experience -= _requiredExperience - _currentExperience;
            }
            else
            {
                _currentExperience += experience;
                experience = 0;
            }
        }

		Helper.GetGUIManager().GameStats_SetExperienceBarValue((float)_currentExperience / (float)_requiredExperience);
        Helper.GetGUIManager().GameStats_SetLevelText(_level);
        Helper.GetGUIManager().GameStats_SetExperienceValue(_currentExperience, _requiredExperience);
	}
}
