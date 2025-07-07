using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class PackageExportHelper
{
	private const string UNITY_DEFAULT_PACKAGE_NAME = "nonamed.unitypackage";
	private const string UNITY_PACKAGE_EXTENSTION = ".unitypackage";

	private List<string> _packagingFileList = null;

	public PackageExportHelper()
	{
		_packagingFileList = new List<string>();
	}

	public void ExportPackage(string packageName, string path = null)
	{
		string _packageName = null;

		if (string.IsNullOrEmpty(packageName))
			_packageName = UNITY_DEFAULT_PACKAGE_NAME;
		else
			_packageName = packageName;

		if (!_packageName.Contains(UNITY_PACKAGE_EXTENSTION))
			_packageName = _packageName + UNITY_PACKAGE_EXTENSTION;

		if (path.EndsWith("/"))
			_packageName = path + _packageName;
		else
			_packageName = path + "/" + _packageName;

		AssetDatabase.ExportPackage(_packagingFileList.ToArray(), _packageName, ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
	}

    public void IncludeDirectoriesAndFiles(params string[] paths)
    {
        foreach (var path in paths)
        {
            string _path = ReplaceDirectorySeparator(path);
            _packagingFileList.Add(_path);

            string[] files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
            string[] directories = Directory.GetDirectories(_path, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                if (!file.Contains(".meta") && !file.Contains(".svn"))
                    _packagingFileList.Add(file);
            }

            foreach (var dir in directories)
            {
                if (!dir.Contains(".svn"))
                    _packagingFileList.Add(dir);
            }
        }
    }

    public void ExcludeDirectoriesAndFiles(params string[] paths)
    {
        foreach (var path in paths)
        {
            string _path = ReplaceDirectorySeparator(path);
            if (_packagingFileList.Contains(_path))
                _packagingFileList.Remove(_path);

            string[] files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
            string[] directories = Directory.GetDirectories(_path, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                if (_packagingFileList.Contains(file))
                    _packagingFileList.Remove(file);
            }

            foreach (var dir in directories)
            {
                if (_packagingFileList.Contains(dir))
                    _packagingFileList.Remove(dir);
            }
        }
    }

    public void IncludeFiles(params string[] includeFiles)
	{
		foreach (var includeFile in includeFiles)
		{
			_packagingFileList.Add(ReplaceDirectorySeparator(includeFile));
		}
	}

	public void ExcludeFiles(params string[] excludeFiles)
	{
		foreach (var excludeFile in excludeFiles)
		{
			string path = ReplaceDirectorySeparator(excludeFile);
			if (_packagingFileList.Contains(path))
				_packagingFileList.Remove(path);
		}
	}

	private string ReplaceDirectorySeparator(string path)
	{
#if UNITY_EDITOR_OSX
		char separator = '\\';	// Window separator
#else
		char separator = '/';	// MAC OSX separator
#endif
		return path.Replace(separator, Path.DirectorySeparatorChar);
	}
}
