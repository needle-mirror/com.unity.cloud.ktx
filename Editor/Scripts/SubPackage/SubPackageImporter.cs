// SPDX-FileCopyrightText: 2023 Unity Technologies and the KTX for Unity authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Assertions;

using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace SubPackage
{
    static class SubPackageImporter
    {
#if !DISABLE_SUB_PACKAGE_LOAD
        [InitializeOnLoadMethod]
#endif
        [MenuItem("Help/Configure KTX Sub Packages")]
        static async Task ConfigureSubPackagesAsync()
        {
            try
            {
                var config = SubPackageConfiguration.config;

                var installedPackages = await GetAllInstalledPackagesAsync();
                var subPackages = GetSubPackages(config, installedPackages);
                var expectedPackage = GetSubPackage(config);

                if (subPackages.Count != 1 || subPackages[0].name != expectedPackage.name || subPackages[0].version != expectedPackage.version)
                {
                    DisplayDialog(config);

                    var packagesToRemove = subPackages
                        .Select(p => p.name)
                        .Where(name => name != expectedPackage.name);

                    await ConfigurePackagesAsync(expectedPackage, packagesToRemove);
                }
            }
            catch (System.Exception e)
            {
                //  Explicit logging is required to avoid silent failures due to this task
                //  being triggered as fire and forget.
                Debug.LogException(e);
            }
        }

        static async Task ConfigurePackagesAsync(SubPackageEntrySchema expected, IEnumerable<string> remove)
        {
#if UNITY_2021_2_OR_NEWER
            await AddAndRemoveAsync(new string[] { expected.fullName }, remove.ToArray());
#else
            foreach (var package in remove)
            {
                await RemoveAsync(package);
            }

            await AddAsync(expected.fullName);
#endif
        }

        static async Task AddAsync(string package)
        {
            var result = Client.Add(package);

            while (!result.IsCompleted)
                await Yield();

            if (result.Status != StatusCode.Success)
                Debug.LogError(result.Error.message);
        }

        static async Task RemoveAsync(string package)
        {
            var result = Client.Remove(package);

            while (!result.IsCompleted)
                await Yield();

            if (result.Status != StatusCode.Success)
                Debug.LogError(result.Error.message);
        }

#if UNITY_2021_2_OR_NEWER
        static async Task AddAndRemoveAsync(string[] add, string[] remove)
        {
            var result = Client.AddAndRemove(add, remove);

            while (!result.IsCompleted)
                await Yield();

            if (result.Status != StatusCode.Success)
                Debug.LogError(result.Error.message);
        }
#endif

        static async Task<List<PackageInfo>> GetAllInstalledPackagesAsync()
        {
            var request = Client.List(offlineMode: true, includeIndirectDependencies: false);

            while (!request.IsCompleted)
                await Yield();

            Assert.AreEqual(StatusCode.Success, request.Status);

            return request.Result.ToList();
        }

        static List<PackageInfo> GetSubPackages(SubPackageConfigSchema config, List<PackageInfo> installedPackages)
        {
            Regex regex = new Regex(config.cleanupRegex);

            return installedPackages
                .Where(package => regex.IsMatch(package.name))
                .ToList();
        }

        static SubPackageEntrySchema GetSubPackage(SubPackageConfigSchema config)
        {
            var unityVersion = new UnityVersion(Application.unityVersion);

            foreach (var subPackage in config.subPackages)
            {
                var minimumVersion = new UnityVersion(subPackage.minimumUnityVersion);

                if (minimumVersion <= unityVersion)
                {
                    return subPackage;
                }
            }

            throw new System.InvalidOperationException("Could not find a version of the binaries to match the current version of Unity");
        }

        static void DisplayDialog(SubPackageConfigSchema config)
        {
            if (Application.isBatchMode)
                return;

            EditorUtility.DisplayDialog(config.dialogTitle, config.dialogText, "Ok");
        }

        static async Task Yield()
        {
            if (Application.isBatchMode)
                Thread.Sleep(10);
            else
                await Task.Yield();
        }
    }
}
