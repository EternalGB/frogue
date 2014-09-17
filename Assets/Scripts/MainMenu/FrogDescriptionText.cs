using UnityEngine;
using System.Collections.Generic;

public class FrogDescriptionText : MonoBehaviour
{

	public static Dictionary<string,string> displayNames = new Dictionary<string, string>
	{
		{"FrogClassic","Frog Classic"},
		{"FlyingFrog","Flying Frog"},
		{"SpittingFrog","Spitting Frog"},
		{"FrogJetpack","Jetpack Frog"},
		{"FrogMutant","Mutant Frog"}
	};

	public static Dictionary<string,List<string>> properties = new Dictionary<string, List<string>>
	{
		{"FrogClassic", new List<string>{
				"Standard jump",
				"Standard tongue"
			}},
		{"FlyingFrog",new List<string>{
				"+Gliding ability",
				"-Shorter tongue"
			}},
		{"SpittingFrog",new List<string>{
				"+Can launch spit to disable obstacles temporarily",
				"-No tongue"
			}},
		{"FrogJetpack",new List<string>{
				"+Can jump once while in the air",
				"-Tongue pulls less strongly"
			}},
		{"FrogMutant",new List<string>{
				"+Two tongues",
				"+Tongues slightly longer than normal",
				"-Shorter jump"
			}}
	};

}

