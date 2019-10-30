using Assets.scripts.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
    public class SetSceneData
    {

        public List<SceneData> LocalSceneList = new List<SceneData>();

        public SetSceneData()
        {
            MakeStoryDescription();
            MakeDialogScenes();
            MakeGameQuestionAndAnswers();
        }

        private void MakeStoryDescription()
        {
            string descriptionScene = "\t\t\t Once there lived a bunny, Jack with his parents Will and Pink in a village close to a furious jungle. In the deep jungle " +
             "there also lived a fearsome monster in the castle who preyed on innocent animals. One day, Will and Pink along with their neighbour Tinku Duck " +
             "went to the jungle in search of fruits, vegetables and firewood. They were not able to find enough fruits in the outer jungle, so they went deep " +
             "into the forest.As they went deep, there was a garden full of fruits and vegetables. They ran into the garden without thinking twice to pick the fruits. " +
             "They are not aware of that it is monster's garden. " + "\n" +
             "\t\t\t They plucked enough fruits and were relaxing in the meadow. By that time the monster came back from his hunting. He saw these three uninvited animals" +
             " trespassed in his garden and found that they took his fruits without his permission. He got angry, rushed towards them caught Will and Pink Somehow Tinku" +
             " managed to escape from the monster and rushed to the village.";
            SceneData sceneObj = new SceneData();
            sceneObj.SceneId = 1111;
            sceneObj.Question = descriptionScene;
            sceneObj.Answer = "gameHome";
            sceneObj.Level = 0;
            LocalSceneList.Add(sceneObj);
        }

        private void MakeDialogScenes()
        {
            int sceneIndex = 1112;
            List<String> dialogList = new List<string> {
                "Jack, Jack...!!",
                "Hey Tinku. \n What happened? \n You seems to be so terrified.",
                "That monster!! \n He got your parents.",
                "What, What happened?",
                "I am gonna kill that monster and save them. $ Tinku explained what actually happened back in the jungle.",
                "Jack learned from village elders that the monster won't kill his parent but will enslave them till they die. He realized that he can actually save his parents by " +
                    "completing certain tasks which will let him inside the castle. So he marched to the monster's castle."
            };
            dialogList.ForEach(dialog => {
                SceneData sceneObj = new SceneData();
                sceneObj.SceneId = sceneIndex;
                sceneObj.Question = dialog;
                sceneObj.Answer = "dialog" + sceneIndex.ToString();
                sceneObj.Level = 0;
                LocalSceneList.Add(sceneObj);
                sceneIndex++;
            });

        }

        private void MakeGameQuestionAndAnswers()
        {
            List<SceneData> scenelist = new List<SceneData> {
                 new SceneData{
                    SceneId = 1,
                    Question = "What starts with a T, ends with a T, and has T in it.",
                    Answer = "teapot",
                    Level = 1
                },
                new SceneData{
                    SceneId = 2,
                    Question = "I stand when I'm sitting, and jump when I'm walking. Who am I?.",
                    Answer = "kangaroo",
                    Level = 1
                },
                new SceneData{
                    SceneId = 3,
                    Question = "If you drop a yellow hat in the Red Sea what will it become?",
                    Answer = "wet",
                    Level = 1
                },
                new SceneData{
                    SceneId = 4,
                    Question = "There is a board showing special text \n" +
                    "                '4+n05'              \nBreak the text and find direction to move further.",
                    Answer = "south",
                    Level = 2
                },
                 new SceneData{
                    SceneId = 5,
                    Question = "After walking in south direction you reached dead end. You can see a underground door. Open it to get in.",
                    Answer = "open",
                    Level = 2
                },
                 new SceneData{
                    SceneId = 6,
                    Question = "After you stepped into the room there are 4 doors in each direction. You got confused, suddenly you heard a loud voice saying, \n" +
                    "'Follow the direction of the star that won't leave the moon' \n",
                    Answer = "north",
                    Level = 2
                },
                 new SceneData{
                    SceneId = 7,
                    Question = "After opening the correct door you walked and reached a garden with 2 small streams. Thinking which one choose you found a direction board saying \n" +
                    "     'swim to the direction you think is wise \n" +
                    "       North South East West    \n" +
                    "       I won't tell which one is best \n" +
                    "       It is not hot as it is not cold \n" +
                    "       but if you stand in front you will look bold'",
                    Answer = "west",
                    Level = 2
                },
                 new SceneData{
                    SceneId = 8,
                    Question = "Follwing the direction of stream towards west. You reached infornt of a small cave. You went inside and found 2 boxes, one with full of diamonds and other one have a big sharp sword. Choose wisely. \n " +
                    "'Diamond or Sword' ",
                    Answer = "sword",
                    Level = 2
                },
                new SceneData{
                    SceneId = 9,
                    Question = "You are in a cabin and it is pitch black. You have one match on you. Which do you light first, the newspaper, the lamp, the candle, or the fire?",
                    Answer = "match",
                    Level = 3
                },
                new SceneData{
                    SceneId = 10,
                    Question = "Give me food, and I will live; give me water, and I will diw. What am I?",
                    Answer = "fire",
                    Level = 3
                },
                new SceneData{
                    SceneId = 11,
                    Question = "Jimmy's mother had four childres, She named the first Monday. She name the second Tuesday and she name the third Wednesday. What is the name of fourth child?",
                    Answer = "jinny",
                    Level = 3
                },
                new SceneData{
                    SceneId = 12,
                    Question = "What has a mouth but cannot eat. Moves, but has no legs. Has a bank, but no money?",
                    Answer = "river",
                    Level = 3
                },
                 new SceneData{
                    SceneId = 13,
                    Question = "I exists only when there is light, but direct light kills me. What am I?",
                    Answer = "shadow",
                    Level = 3
                },
                new SceneData{
                    SceneId = 14,
                    Question = "You got tired and lost all energy to walk further and without energy you can’t kill the monster. Suddenly you remembered about the special energy drink that gives energy to kill the monster. You know the map that leads to the energy drink and ran to get it. The map won’t show the entire path. There will be five destinations in the way and the map will only show one place and one riddle at a time." +
                                "\nYou checked the map and saw some directions and a riddle… \n"+
                                "\n'You must walk 12 steps to the west and 20 steps to the east, you will find a room. There you’ll have to stand on the thing that is bought by the yarn and worn by the foot'",
                    Answer = "carpet",
                    Level = 4
                },
                new SceneData{
                    SceneId = 15,
                    Question = "As soon as you stood on the carpet you saw that a path has appeared to the next destination. you followed the map and reached a room full of books. The second riddle in the map appeared." +  
                               "\n\n'You measure my life in hours, and I serve you by expiring. I’m quick when I’m thin and slow when I’m fat. The wind is my enemy.'",
                    Answer = "candle",
                    Level = 4
                },
                new SceneData{
                    SceneId = 15,
                    Question = "You lit the candle, the bookshelves moved to different directions and a path appeared in front of you  and it appeared in map too. You took the way  and ended up in a beautiful room. The map directed you to a flower vase with a bunch of flowers. The next riddle appeared. \n\n"+
                                "'Soft and fragile is my skin, I get my growth in mud\n"+
                                   "I’m dangerous as much as pretty, for if not careful, I draw blood'",
                    Answer = "thorn",
                    Level = 4
                },
                new SceneData{
                    SceneId = 16,
                    Question = "You took a thorn. You poked yourself with the thorn and put a drop of blood on the map. Map shown the way to a room where different metal ores were kept. You have to identify the one which map tells \n"+
                                "\n'I drive men mad, \nFor love of me, \nEasily beaten \nNever free",
                    Answer = "gold",
                    Level = 4
                },
                new SceneData{
                    SceneId = 17,
                    Question = "From that room the map took you to a tiny cellar. He saw the blue drink in a jug. But you cannot touch the jar until you wear what the map says \n"+
                                "\n'A mile from end to end, yet as close to as a friend. A precious commodity, freely given. Seen on the dead and on the living. Found on the rich, poor, short and tall, but shared among children most of all.'",
                    Answer = "smile",
                    Level = 4
                },
                new SceneData{
                    SceneId = 18,
                    Question = "The foolish man wastes me, The average man spends me, And wise man invests me, Yet all men succumb to me. What am I? ",
                    Answer = "time",
                    Level = 5
                },
                new SceneData{
                    SceneId = 19,
                    Question = "It can pierce the best armor, And make swords crumble with a rub. Yet for all its power, It can’t harm a club.",
                    Answer = "rust",
                    Level = 5
                },
                new SceneData{
                    SceneId = 20,
                    Question = "Lives without a body, hears without ears, speaks without a mouth, to which the air alone gives birth.",
                    Answer = "echo",
                    Level = 5
                },
                new SceneData{
                    SceneId = 21,
                    Question = "A merchant can place 8 large boxes or 10 small boxes into a carton for shipping. In one shipment, he sent a total of 96 boxes. If there are more large boxes than small boxes, how many cartons did he ship?",
                    Answer = "11",
                    Level = 5
                },
                new SceneData{
                    SceneId = 22,
                    Question = "A sphere has three, a circle has two, and a point has zero. What is it?",
                    Answer = "dimensions",
                    Level = 5
                }

            };
            LocalSceneList.AddRange(scenelist);
        }
    }
}
