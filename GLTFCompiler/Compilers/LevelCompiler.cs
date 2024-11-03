using MagickaForge.GLTF;
using MagickaForge.Pipeline.Levels;
using MagickaForge.Components.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLTFCompiler.Compilers
{
    internal class LevelCompiler
    {
        public LevelCompiler()
        {

        }
        public void Compile(string instructionPath)
        {
            Level level = new Level();
            GLTFModel model;
            model = GLTFModel.LoadGLTFModel(instructionPath);
        }
    }
}
