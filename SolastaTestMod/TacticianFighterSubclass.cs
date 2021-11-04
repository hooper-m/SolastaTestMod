using SolastaModApi;
using SolastaModApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Helpers = SolastaModHelpers.Helpers;
using NewFeatureDefinitions = SolastaModHelpers.NewFeatureDefinitions;

namespace SolastaTestMod {
    class TacticianFighterSubclass {
        const string TacticianFighterSubclassName = "TacticianFighterSubclass";
        const string TacticianFighterSubclassGuid = "893EE2C3-FA57-4E8B-99B4-75AC9D8EE29F";

        public static Guid TF_BASE_GUID = new Guid(TacticianFighterSubclassGuid);

        public static FeatureDefinitionPower tacticAccurateStrike;

        public static void BuildAndAddSubclass() {
            var subclassGuiPresentation = new GuiPresentationBuilder(
                "Subclass/&TacticianFighterSubclassDescription",
                "Subclass/&TacticianFighterSubclassTitle")
                .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.DomainBattle.GuiPresentation.SpriteReference)
                .Build();

            CreateAccurateStrikeTactic();

            var definition = new CharacterSubclassDefinitionBuilder(TacticianFighterSubclassName, TacticianFighterSubclassGuid)
                .SetGuiPresentation(subclassGuiPresentation)
                .AddFeatureAtLevel(tacticAccurateStrike, 3)
                //.AddFeatureAtLevel(blankTestFeature, 3)
                .AddToDB();

            DatabaseHelper.FeatureDefinitionSubclassChoices.SubclassChoiceFighterMartialArchetypes.Subclasses.Add(definition.Name);
        }

        static FeatureDefinition CreateBlankTestFeature() {
            const string name = "BlankTestFeature";
            var blank_feature = Helpers.FeatureBuilder<FeatureDefinition>.createFeature
            (
                name,
                GuidHelper.Create(TF_BASE_GUID, name).ToString(),
                "Feature/&BlankTestFeatureTitle",
                "Feature/&BlankTestFeatureDescription",
                null
            );

            return blank_feature;
        }

        static NewFeatureDefinitions.FeatureDefinitionReactionPowerOnAttackAttempt CreateCounterattackTactic() {
            string counterAttackTacticTitleString = "Feature/&CounterattackTacticTitle";
            string counterAttackTacticDescriptionString = "Feature/&CounterattackTacticDescription";
            string name = "CounterAttackTactic";

            var effect = new EffectDescription();

            var counterAttackTactic = Helpers.FeatureBuilder<NewFeatureDefinitions.FeatureDefinitionReactionPowerOnAttackAttempt>.createFeature(
                name,
                GuidHelper.Create(TF_BASE_GUID, name).ToString(),
                counterAttackTacticTitleString,
                counterAttackTacticDescriptionString,
                SolastaModHelpers.Common.common_no_icon,
                a =>
                {
                    a.worksOnMelee = true;
                    a.worksOnMagic = a.worksOnRanged = false;
                    a.onlyOnFailure = true;
                    a.onlyOnSuccess = false;                    
                }
            );

            return counterAttackTactic;
        }

        static void CreateAccurateStrikeTactic() {

            string accurateStrikeTitle = "Feature/&AccurateStrikeTacticFeatureTitle";
            string accurateStrikeDescriptionString = "Feature/&AccurateStrikeTacticFeatureDescription";
            string accurateStrikeUsePowerTitle = "Feature/&TacticianFighterSubclassAccurateStrikeUsePowerTitle";
            string accurateStrikeUsePowerDescription = "Feature/&TacticianFighterSubclassAccurateStrikeUsePowerDescription";
            string accurateStrikeUseReactTitle = "Reaction/&CommonUsePowerSpendTitle";
            string accurateStrikeUseReactDescription = "Reaction/&TacticanFighterSubclassAccurateStrikePowerReactDescription";

            string attackBonusName = "TacticianFighterSubclassTacticAccurateStrikeAttackBonus";
            string conditionName = "TacticanFighterSubclassTacticAccurateStrikeCondition";
            string powerName = "TacticanFighterSubclassTacticAccurateStrikePower";

            var tacticAccurateStrikeAttackBonus = Helpers.AttackBonusBuilder
                                                         .createAttackBonus(
                                                            attackBonusName,
                                                            GuidHelper.Create(TF_BASE_GUID, attackBonusName).ToString(),
                                                            "TacticianFighterSubclassTacticAccurateStrikeAttackBonusTitle",
                                                            "TacticianFighterSubclassTacticAccurateStrikeAttackBonusDescription",
                                                            null,
                                                            1,
                                                            RuleDefinitions.DieType.D12,    //TODO: Character's weapon die
                                                            false
                                                          );

            var tacticAccurateStrikeCondition = Helpers.ConditionBuilder
                                                       .createConditionWithInterruptions(
                                                            conditionName,
                                                            GuidHelper.Create(TF_BASE_GUID, conditionName).ToString(),
                                                            "TacticanFighterSubclassTacticAccurateStrikeConditionTitle",
                                                            "TacticanFighterSubclassTacticAccurateStrikeConditionDescription",
                                                            null,
                                                            DatabaseHelper.ConditionDefinitions.ConditionShieldedByFaith,
                                                            new RuleDefinitions.ConditionInterruption[] { RuleDefinitions.ConditionInterruption.Attacks },
                                                            tacticAccurateStrikeAttackBonus
                                                       );
            
            NewFeatureDefinitions.ConditionsData.no_refresh_conditions.Add(tacticAccurateStrikeCondition);
            tacticAccurateStrikeCondition.SetSilentWhenAdded(true);
            tacticAccurateStrikeCondition.SetSilentWhenRemoved(true);

            var effect = new EffectDescription();
            effect.Copy(DatabaseHelper.SpellDefinitions.DivineFavor.EffectDescription);
            effect.SetRangeType(RuleDefinitions.RangeType.Self);
            effect.SetTargetType(RuleDefinitions.TargetType.Self);
            effect.SetRangeParameter(1);
            effect.SetTargetParameter(1);
            effect.SetTargetParameter2(2);
            effect.DurationParameter = -1;
            effect.SetTargetSide(RuleDefinitions.Side.Ally);
            effect.DurationType = RuleDefinitions.DurationType.Round;
            effect.EffectForms.Clear();

            var effectForm = new EffectForm();
            effectForm.ConditionForm = new ConditionForm();
            effectForm.FormType = EffectForm.EffectFormType.Condition;
            effectForm.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            effectForm.ConditionForm.ConditionDefinition = tacticAccurateStrikeCondition;
            effect.EffectForms.Add(effectForm);

            var tacticAccurateStrikePower = Helpers.GenericPowerBuilder<NewFeatureDefinitions.FeatureDefinitionReactionPowerOnAttackAttempt>
                                                   .createPower(
                                                        powerName,
                                                        GuidHelper.Create(TF_BASE_GUID, powerName).ToString(),
                                                        accurateStrikeTitle,
                                                        accurateStrikeDescriptionString,
                                                        DatabaseHelper.SpellDefinitions.DivineFavor.GuiPresentation.SpriteReference,
                                                        effect,
                                                        RuleDefinitions.ActivationTime.NoCost,
                                                        4,  // update to power linked with other tactics
                                                        RuleDefinitions.UsesDetermination.Fixed,
                                                        RuleDefinitions.RechargeRate.ShortRest
                                                    );

            tacticAccurateStrikePower.worksOnMelee = true;
            tacticAccurateStrikePower.worksOnRanged = true;
            tacticAccurateStrikePower.onlyOnFailure = true;
            tacticAccurateStrikePower.worksOnMagic = false;
            tacticAccurateStrikePower.SetShortTitleOverride(accurateStrikeTitle);

            Helpers.StringProcessing.addPowerReactStrings(
                tacticAccurateStrikePower,
                accurateStrikeUsePowerTitle,        // reaction box title
                accurateStrikeUsePowerDescription,  // reaction box description
                accurateStrikeUseReactTitle,        // 'spend' button tooltip
                accurateStrikeUseReactDescription
            );

            tacticAccurateStrike = tacticAccurateStrikePower;
        }
    }
}
