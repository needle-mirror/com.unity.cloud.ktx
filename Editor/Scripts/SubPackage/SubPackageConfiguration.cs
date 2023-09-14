// SPDX-FileCopyrightText: 2023 Unity Technologies and the KTX for Unity authors
// SPDX-License-Identifier: Apache-2.0

using UnityEngine;

namespace SubPackage
{
    static class SubPackageConfiguration
    {
        const string k_PackageName = "KTX for Unity";

        internal static SubPackageConfigSchema config = new SubPackageConfigSchema()
        {
            dialogTitle = "Installing Sub Packages",
            dialogText = $"The {k_PackageName} package requires sub-packages which vary, depending on the Unity version. These dependencies will now be updated automatically and will appear in your project's manifest file.",

            cleanupRegex = @"^com\.unity\.cloud\.ktx\.webgl-.*$",
            
            subPackages = new[]
            {
                new SubPackageEntrySchema()
                {
                    name = "com.unity.cloud.ktx.webgl-2023",
                    minimumUnityVersion = "2023.2.0a17",
                    version = "1.0.0"
                },
                new SubPackageEntrySchema()
                {
                    name = "com.unity.cloud.ktx.webgl-2022",
                    minimumUnityVersion = "2022.2.0",
                    version = "1.0.0"
                },
                new SubPackageEntrySchema()
                {
                    name = "com.unity.cloud.ktx.webgl-2021",
                    minimumUnityVersion = "2021.2.0",
                    version = "1.0.0"
                },
                new SubPackageEntrySchema()
                {
                    name = "com.unity.cloud.ktx.webgl-2020",
                    minimumUnityVersion = "2019.2.0",
                    version = "1.0.0"
                }
            }
        };
    }
}
