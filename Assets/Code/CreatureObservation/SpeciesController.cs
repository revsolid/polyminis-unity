using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesController : MonoBehaviour {

	Dictionary<int, Creature> Individuals = new Dictionary<int, Creature>();
	List<SpeciesStep> Steps = new List<SpeciesStep>();
	public Creature CreaturePrototype;
	// Use this for initialization
	void Start ()
	{
		IndividualModel im = JsonUtility.FromJson<IndividualModel>( 
		"{\"Control\":{\"HiddenToOutput\":[0.5316351652145386,0.2435460090637207,0.48975837230682373,0.26685452461242676,0.3977762460708618],\"InToHidden\":[0.6227080821990967,0.8990602493286133,0.556668758392334,0.6417162418365479,0.6351940631866455,0.795133113861084,0.9386725425720215,0.315014123916626,0.7965415716171265,0.2110811471939087,0.6135859489440918,0.9092851877212524,0.45118772983551025,0.7212232351303101,0.40727102756500244,0.785159707069397,0.09202146530151367,0.9536818265914917,0.5793490409851074,0.21268653869628906]},\"ID\":20,\"Morphology\":{\"Body\":{\"(0, 0)\":{\"TID\":113,\"Tier\":\"TierI\"},\"(0, 1)\":{\"TID\":37,\"Tier\":\"TierI\"},\"(1, 0)\":{\"TID\":220,\"Tier\":\"TierI\"},\"(1, 1)\":{\"TID\":140,\"Tier\":\"TierI\"},\"(2, 0)\":{\"TID\":156,\"Tier\":\"TierI\"},\"(2, 1)\":{\"TID\":2,\"Tier\":\"TierI\"},\"(2, 2)\":{\"TID\":59,\"Tier\":\"TierI\"}},\"Chromosome\":[[131,170,106,69],[239,205,37,36],[135,242,220,193],[235,55,113,243],[236,154,156,139],[50,240,2,244],[173,78,59,248],[238,37,140,255]]},\"Physics\":{\"Dimensions\":{\"x\":3.0,\"y\":3.0},\"ID\":20,\"StartingPos\":{\"x\":95.0,\"y\":15.0}}}");
		
		Debug.Log(im.ID);
		Debug.Log(im.Physics.StartingPos);
		Debug.Log(im.Physics.Dimensions);
		Debug.Log(im.Morphology);
		Debug.Log(im.HP);
		Debug.Log(im.Temperature);
		Debug.Log(im.Ph);
		
		SpawnCreature(im);
		
		
		SpeciesStep step = JsonUtility.FromJson<SpeciesStep>( 
			"{\"Individuals\": [{\"Control\":{\"Hidden\":[0.8587784767150879,0.8673220872879028,0.9168651103973389,0.8060646653175354,0.7770862579345703],\"Inputs\":[0.9599999785423279,0.15000000596046448,0.0,1.0],\"Outputs\":[0.8759307861328125]},\"ID\":20,\"Physics\":{\"Collisions\":[],\"ID\":20,\"LastAction\":{\"direction\":\"HORIZONTAL\",\"impulse\":0.8759307861328125},\"Orientation\":\"UP\",\"Position\":{\"x\": 95.0, \"y\": 14.0}}},{\"Control\":{\"Hidden\":[0.7421139478683472,0.9110366702079773,0.8629021048545837,0.8136493563652039,0.9233283400535583,0.7421475648880005],\"Inputs\":[0.5099999904632568,0.6600000262260437,0.0,1.0],\"Outputs\":[]},\"ID\":21,\"Physics\":{\"Collisions\":[],\"ID\":21,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":{\"x\": 51.0, \"y\": 66.0}}},{\"Control\":{\"Hidden\":[0.8810168504714966,0.904151201248169,0.7901993989944458],\"Inputs\":[0.949999988079071,0.41999998688697815,0.0,1.0],\"Outputs\":[]},\"ID\":22,\"Physics\":{\"Collisions\":[],\"ID\":22,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":{\"x\": 94.0, \"y\": 41.0}}},{\"Control\":{\"Hidden\":[0.8441071510314941,0.8253834247589111,0.8025809526443481,0.8056054711341858,0.8451812267303467],\"Inputs\":[0.550000011920929,0.23000000417232513,0.0,1.0],\"Outputs\":[]},\"ID\":23,\"Physics\":{\"Collisions\":[],\"ID\":23,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[55.0,23.0]}},{\"Control\":{\"Hidden\":[0.7498294115066528,0.7939981818199158,0.893932044506073,0.8046205043792725],\"Inputs\":[0.47999998927116394,0.8500000238418579,0.0,1.0],\"Outputs\":[]},\"ID\":24,\"Physics\":{\"Collisions\":[],\"ID\":24,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[48.0,85.0]}},{\"Control\":{\"Hidden\":[0.7467555999755859,0.9091744422912598,0.7482597231864929],\"Inputs\":[0.9800000190734863,0.6100000143051147,0.0,1.0],\"Outputs\":[]},\"ID\":25,\"Physics\":{\"Collisions\":[],\"ID\":25,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[93.0,59.0]}},{\"Control\":{\"Hidden\":[0.8780509233474731,0.7473177909851074,0.8105037212371826,0.8716261982917786,0.8878514766693115],\"Inputs\":[0.8500000238418579,0.14000000059604645,0.0,1.0],\"Outputs\":[]},\"ID\":26,\"Physics\":{\"Collisions\":[],\"ID\":26,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[85.0,14.0]}},{\"Control\":{\"Hidden\":[0.7030810117721558,0.77083420753479,0.7496645450592041,0.8376195430755615],\"Inputs\":[0.029999999329447746,0.2800000011920929,0.0,1.0],\"Outputs\":[]},\"ID\":27,\"Physics\":{\"Collisions\":[],\"ID\":27,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[3.0,28.0]}},{\"Control\":{\"Hidden\":[0.8420175313949585,0.7843615412712097,0.7104517221450806,0.858683168888092,0.8055749535560608,0.7775624394416809],\"Inputs\":[0.38999998569488525,0.6700000166893005,0.0,1.0],\"Outputs\":[]},\"ID\":28,\"Physics\":{\"Collisions\":[],\"ID\":28,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[39.0,67.0]}},{\"Control\":{\"Hidden\":[0.6934688687324524,0.9085155129432678,0.6455265283584595],\"Inputs\":[0.7900000214576721,0.6100000143051147,0.0,1.0],\"Outputs\":[]},\"ID\":29,\"Physics\":{\"Collisions\":[],\"ID\":29,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[79.0,61.0]}}]}"
		);
		
		AddStep(step);
		

		step = JsonUtility.FromJson<SpeciesStep>( 
			"{\"Individuals\": [{\"Control\":{\"Hidden\":[0.8587784767150879,0.8673220872879028,0.9168651103973389,0.8060646653175354,0.7770862579345703],\"Inputs\":[0.9599999785423279,0.15000000596046448,0.0,1.0],\"Outputs\":[0.8759307861328125]},\"ID\":20,\"Physics\":{\"Collisions\":[],\"ID\":20,\"LastAction\":{\"direction\":\"HORIZONTAL\",\"impulse\":0.8759307861328125},\"Orientation\":\"UP\",\"Position\":{\"x\": 95.0, \"y\": 13.0}}},{\"Control\":{\"Hidden\":[0.7421139478683472,0.9110366702079773,0.8629021048545837,0.8136493563652039,0.9233283400535583,0.7421475648880005],\"Inputs\":[0.5099999904632568,0.6600000262260437,0.0,1.0],\"Outputs\":[]},\"ID\":21,\"Physics\":{\"Collisions\":[],\"ID\":21,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":{\"x\": 51.0, \"y\": 66.0}}},{\"Control\":{\"Hidden\":[0.8810168504714966,0.904151201248169,0.7901993989944458],\"Inputs\":[0.949999988079071,0.41999998688697815,0.0,1.0],\"Outputs\":[]},\"ID\":22,\"Physics\":{\"Collisions\":[],\"ID\":22,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":{\"x\": 94.0, \"y\": 41.0}}},{\"Control\":{\"Hidden\":[0.8441071510314941,0.8253834247589111,0.8025809526443481,0.8056054711341858,0.8451812267303467],\"Inputs\":[0.550000011920929,0.23000000417232513,0.0,1.0],\"Outputs\":[]},\"ID\":23,\"Physics\":{\"Collisions\":[],\"ID\":23,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[55.0,23.0]}},{\"Control\":{\"Hidden\":[0.7498294115066528,0.7939981818199158,0.893932044506073,0.8046205043792725],\"Inputs\":[0.47999998927116394,0.8500000238418579,0.0,1.0],\"Outputs\":[]},\"ID\":24,\"Physics\":{\"Collisions\":[],\"ID\":24,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[48.0,85.0]}},{\"Control\":{\"Hidden\":[0.7467555999755859,0.9091744422912598,0.7482597231864929],\"Inputs\":[0.9800000190734863,0.6100000143051147,0.0,1.0],\"Outputs\":[]},\"ID\":25,\"Physics\":{\"Collisions\":[],\"ID\":25,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[93.0,59.0]}},{\"Control\":{\"Hidden\":[0.8780509233474731,0.7473177909851074,0.8105037212371826,0.8716261982917786,0.8878514766693115],\"Inputs\":[0.8500000238418579,0.14000000059604645,0.0,1.0],\"Outputs\":[]},\"ID\":26,\"Physics\":{\"Collisions\":[],\"ID\":26,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[85.0,14.0]}},{\"Control\":{\"Hidden\":[0.7030810117721558,0.77083420753479,0.7496645450592041,0.8376195430755615],\"Inputs\":[0.029999999329447746,0.2800000011920929,0.0,1.0],\"Outputs\":[]},\"ID\":27,\"Physics\":{\"Collisions\":[],\"ID\":27,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[3.0,28.0]}},{\"Control\":{\"Hidden\":[0.8420175313949585,0.7843615412712097,0.7104517221450806,0.858683168888092,0.8055749535560608,0.7775624394416809],\"Inputs\":[0.38999998569488525,0.6700000166893005,0.0,1.0],\"Outputs\":[]},\"ID\":28,\"Physics\":{\"Collisions\":[],\"ID\":28,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[39.0,67.0]}},{\"Control\":{\"Hidden\":[0.6934688687324524,0.9085155129432678,0.6455265283584595],\"Inputs\":[0.7900000214576721,0.6100000143051147,0.0,1.0],\"Outputs\":[]},\"ID\":29,\"Physics\":{\"Collisions\":[],\"ID\":29,\"LastAction\":{},\"Orientation\":\"UP\",\"Position\":[79.0,61.0]}}]}"
		);
		AddStep(step);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		while (Steps.Count > 0)
		{
			SpeciesStep ss = Steps[0];
			Steps.RemoveAt(0);
			foreach (IndividualStep i_s in  ss.Individuals)
			{
				Creature ind = null;
				Individuals.TryGetValue(i_s.ID, out ind);
				if (ind != null)
				{
					Debug.Log(i_s.Physics.Position);
					ind.AddStep(i_s);
				}
				else
				{
					
				}
			}
		}
	}
	
	void SpawnCreature(IndividualModel model)	
	{
		Creature creature = Instantiate<Creature>(CreaturePrototype);
		creature.SetDataFromModel(model);
		Individuals[model.ID] = creature;
	}
	
	void AddStep(SpeciesStep ss)
	{
		Steps.Add(ss);
	}
}
