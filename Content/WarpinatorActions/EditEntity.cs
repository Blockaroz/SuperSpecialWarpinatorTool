using SuperSpecialWarpinatorTool.Content.WarpMenuElements;
using SuperSpecialWarpinatorTool.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SuperSpecialWarpinatorTool.Content.WarpinatorActions
{
    public class EditEntity : WarpinatorAction
    {
        private Ref<Entity> entity = new Ref<Entity>();

        private Ref<string> search = new Ref<string>();

        public override void SetDefaults()
        {
            entity.Value = null;
            search.Value = "";
        }

        public override List<IWarpMenuElement> AddMenuElements() => new List<IWarpMenuElement>()
        {
            new EntitySelector(entity)
        };
    }
}
