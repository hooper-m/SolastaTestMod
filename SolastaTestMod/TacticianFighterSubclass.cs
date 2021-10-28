using SolastaModApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaTestMod {
    class TacticianFighterSubclass {
        const string TacticianFighterSubclassName = "TacticianFighterSubclass";
        const string TacticianFighterSubclassGuid = "893EE2C3-FA57-4E8B-99B4-75AC9D8EE29F";

        public static Guid TF_BASE_GUID = new Guid(TacticianFighterSubclassGuid);

        //TODO Feature definitions?

        public static void BuildAndAddSubclass() {
            var subclassGuiPresentation = new GuiPresentationBuilder(
                "Subclass/&TacticianFighterSubclassDescription",
                "Subclass/&TacticianFighterSubclassTitle")
                .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.DomainBattle.GuiPresentation.SpriteReference)
                .Build();

            var definition = new CharacterSubclassDefinitionBuilder(TacticianFighterSubclassName, TacticianFighterSubclassGuid)
                .SetGuiPresentation(subclassGuiPresentation)
                //AddFeatures
                .AddToDB();

            DatabaseHelper.FeatureDefinitionSubclassChoices.SubclassChoiceFighterMartialArchetypes.Subclasses.Add(definition.Name);
        }
    }
}
