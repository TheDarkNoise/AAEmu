﻿using System;
using System.Collections.Generic;

using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Items;
using AAEmu.Game.Models.Game.Items.Actions;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Utils;

using NLog;

namespace AAEmu.Game.Models.Game.Skills.Effects.SpecialEffects
{
    public class Skinize : SpecialEffectAction
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public override void Execute(Unit caster,
            SkillCaster casterObj,
            BaseUnit target,
            SkillCastTarget targetObj,
            CastAction castObj,
            Skill skill,
            SkillObject skillObject,
            DateTime time,
            int value1,
            int value2,
            int value3,
            int value4)
        {
            _log.Warn("value1 {0}, value2 {1}, value3 {2}, value4 {3}", value1, value2, value3, value4);

            Character character = (Character)caster;
            if (character == null)
            {
                return;
            }

            SkillCastItemTarget itemTarget = (SkillCastItemTarget)targetObj;
            if (itemTarget == null)
            {
                return;
            }

            Item itemToImage = character.Inventory.GetItem(itemTarget.Id);
            if (itemToImage == null)
            {
                return;
            }

            SkillItem powderSkillItem = (SkillItem)casterObj;
            if (powderSkillItem == null)
            {
                return;
            }

            Item powderItem = character.Inventory.GetItem(powderSkillItem.ItemId);
            if (powderItem == null)
            {
                return;
            }

            if (powderItem.Count < 1)
            {
                return;
            }

            var removeItemTasks = new List<ItemTask>();
            var updateItemTasks = new List<ItemTask>();

            removeItemTasks.Add(InventoryHelper.GetTaskAndRemoveItem(character, powderItem, 1));

            itemToImage.SetFlag(ItemFlag.Skinized);
            updateItemTasks.Add(new ItemUpdateBits(itemToImage));

            character.SendPacket(new SCItemTaskSuccessPacket(ItemTaskType.Sknize, updateItemTasks, new List<ulong>()));
            character.SendPacket(new SCItemTaskSuccessPacket(ItemTaskType.SkillReagents, removeItemTasks, new List<ulong>()));
        }
    }
}