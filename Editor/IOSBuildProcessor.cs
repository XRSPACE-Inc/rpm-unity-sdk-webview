#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace ReadyPlayerMe.WebView.Editor
{
    public class IOSBuildProcessor
    {
        [PostProcessBuild(100)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (BuildTarget.iOS != target) return;
            var projectPath = $"{pathToBuiltProject}/Unity-iPhone.xcodeproj/project.pbxproj";
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projectPath));
#if UNITY_2019_3_OR_NEWER
            string targetGuid = proj.GetUnityMainTargetGuid();
#else
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif
            proj.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
            File.WriteAllText(projectPath, proj.WriteToString());
        }
    }
}
#endif
