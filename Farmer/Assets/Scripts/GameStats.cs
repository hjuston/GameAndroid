using UnityEngine;
using System.Collections;

public class GameStats : MonoBehaviour {

	private int _level = 1;
	public int _requiredExperience = 100;
	public int _currentExperience = 0;
	
	public void AddExperience(int experience)
	{
		_currentExperience += experience;
		if(_currentExperience >= _requiredExperience)
		{
			_level++;
			_requiredExperience *= 2;
			_currentExperience = 0;
		}

		Helper.GetGUIManager().SetExperiencePanelValue((float)_currentExperience / (float)_requiredExperience);
	}
}
