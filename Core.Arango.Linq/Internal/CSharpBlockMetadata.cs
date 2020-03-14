using static Core.Arango.Linq.Internal.CSharpMultilineBlockTypes;

namespace Core.Arango.Linq.Internal
{
    internal class CSharpBlockMetadata
    {
        internal CSharpMultilineBlockTypes BlockType { get; private set; } = Inline;
        internal bool ParentIsBlock { get; private set; }

        internal static CSharpBlockMetadata CreateMetadata(CSharpMultilineBlockTypes blockType = Inline,
            bool parentIsBlock = false)
        {
            return new CSharpBlockMetadata
            {
                BlockType = blockType,
                ParentIsBlock = parentIsBlock
            };
        }

        internal void Deconstruct(out CSharpMultilineBlockTypes blockType, out bool parentIsBlock)
        {
            blockType = BlockType;
            parentIsBlock = ParentIsBlock;
        }
    }
}