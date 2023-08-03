using System;
using System.Collections.Generic;
using AtOSkinExtender;
using BepInEx;
using System.Security;
using System.Security.Permissions;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Rendering.LookDev;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace WolfWarsSkinTweak
{
    [BepInPlugin("com.DestroyedClone.WolfWarsSkinTweak", "Wolf Wars Skin Tweak", "0.1.0")]
    [BepInDependency(AtOSkinExtender.Plugin.modGuid)]
    [BepInDependency("com.DestroyedClone.FoeFacade", BepInDependency.DependencyFlags.SoftDependency)]

    public class ModPlugin : BaseUnityPlugin
    {
        //youngskins cause youngbinks etc
        //past skins? needs to be expandable if theres gonna be new settings
        public const string identifier = "WolfWarsSkinTweak";
        public static List<SkinData> mySkins = new List<SkinData>();
        public static BepInEx.Logging.ManualLogSource _logger;

        public static ConfigEntry<bool> CfgRenameWolfWarSkins;

        public static bool modloaded_foefacade = false;

        public void Awake()
        {
            _logger = Logger;

            CfgRenameWolfWarSkins = Config.Bind("", "Rename Wolfwar Skins", true, "If true, then the wolf war skins will be renamed from the generic The wolf wars to more appropriate names.");


            AtOSkinExtender.Plugin.onCreateSkins += CreateEnemySkinsForHeroes;

            modloaded_foefacade = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.FoeFacade");
            On.MainMenuManager.Start += MainMenuManager_Start;
        }

        private void MainMenuManager_Start(On.MainMenuManager.orig_Start orig, MainMenuManager self)
        {
            orig(self);
            RenameWolfWarSkins();
        }

        //youngbinks	Binks = ranger
        //youngcharls	Charls = healer+lightingspell
        //youngheiner	Model C
        //youngmagnus	Magnus
        //youngottis	Ottis
        //youngyogger	Yogger

        public SkinData DuplicateSkinForSubclass(SubClassData subClassData, SkinData skinToCopy, string sku = "2325780")
        {
            return Assets.CreateSkinData(subClassData.Id, $"{subClassData.SubClassName}"+skinToCopy.SkinName, 0,
                skinToCopy.SpritePortrait, skinToCopy.SpritePortraitGrande,
                skinToCopy.SpriteSilueta, skinToCopy.SpriteSiluetaGrande,
                skinToCopy.SkinGo, sku); //wolfwars SKU
        }

        //[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        //spriteBorder = large portrait
        //spriteBorderSmall = small portrait

        public SkinData CreateSkinFromSubclassForSubclass(SubClassData guyToCopyFrom, SubClassData subToCopyTo, string sku = null)
        {
            return Assets.CreateSkinData(subToCopyTo.Id,
                guyToCopyFrom.CharacterName,// + $" ({subToCopyTo.CharacterName})",
                $"{subToCopyTo.SubClassName}" + "Skin" + guyToCopyFrom.SubClassName,
                0,
                guyToCopyFrom.SpriteSpeed, guyToCopyFrom.SpritePortrait,
                guyToCopyFrom.SpriteBorderSmall, guyToCopyFrom.SpriteBorder,
                guyToCopyFrom.GameObjectAnimated, sku ?? guyToCopyFrom.Sku); //wolfwars SKU
        }

        public SkinData CreateSkinFromNPCForSubclass(NPCData npc, SubClassData subclass, string sku = null)
        {
            return AtOSkinExtender.Assets.CreateSkinData(subclass.Id,
                        npc.NPCName,
                        $"{subclass.Id}Skin{npc.GameObjectAnimated.name}",
                        0,
                        npc.SpriteSpeed,
                        npc.SpritePortrait,
                        npc.SpriteSpeed,
                        npc.SpritePortrait,
                        npc.GameObjectAnimated,
                        sku ?? "");
        }

        private void CreateEnemySkinsForHeroes()
        {
            //CreateRusty();
            //add to shooters
            foreach (var subclass in Globals.Instance._SubClassSource)
            {
                //they dont HAVE skins
                //var binksSkin = Globals.Instance.GetSkinsBySubclass("youngbinks")[0];
                //var charlsSkin = Globals.Instance.GetSkinsBySubclass("youngcharls")[0];
                //_logger.LogMessage(subclass.Value.SubClassName);
                switch (subclass.Value.HeroClass)
                {
                    case Enums.HeroClass.Warrior:
                        break;
                    case Enums.HeroClass.Mage:
                        break;
                    case Enums.HeroClass.Healer:
                        mySkins.Add(CreateSkinFromSubclassForSubclass(Globals.Instance.GetSubClassData("youngcharls"), subclass.Value, AtOSkinExtender.Assets.sku_WolfWars));
                        break;
                    case Enums.HeroClass.Scout:
                        mySkins.Add(CreateSkinFromSubclassForSubclass(Globals.Instance.GetSubClassData("youngbinks"), subclass.Value));
                        break;
                    case Enums.HeroClass.MagicKnight: //unused
                        break;
                    case Enums.HeroClass.Monster: //seemingly used for corruptions/petmonsters(?)
                        break;
                }
            }
            RegisterSkins();
        }

        private void RenameWolfWarSkins()
        {
            if (!CfgRenameWolfWarSkins.Value) return;

            //Globals.Instance.GetSkinData("magnuswolfwars").SkinName = Texts.Instance.GetText("wolfWars");
            Globals.Instance.GetSkinData("heinerwolfwars").SkinName = Globals.Instance.GetSubClassData("youngheiner").CharacterName;
            Globals.Instance.GetSkinData("breewolfwars").SkinName = Globals.Instance.GetNPC("balina").NPCName;
            //Globals.Instance.GetSkinData("ottiswolfwars").SkinName = Texts.Instance.GetText("wolfWars");
        }

        //would have to do flip code and ehhhhh
        /*private void CreateRusty()
        {
            if (modloaded_foefacade)
                return;
            mySkins.Add(CreateSkinFromNPCForSubclass(Globals.Instance.GetNPC("rusty"), Globals.Instance.GetSubClassData("sentinel")));
        }*/

        private void RegisterSkins()
        {
            AtOSkinExtender.Assets.AddSkinDataToPack(mySkins, identifier);
        }
    }
}
