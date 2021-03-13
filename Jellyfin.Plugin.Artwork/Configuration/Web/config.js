export default function (view) {
    const Artwork = {
        pluginId: "7871D3B1-F1B9-4318-9C27-F35998FFBBCC",
        configurationWrapper: document.querySelector("#configurationWrapper"),

        template: document.querySelector("#template-repository"),
        btnAddRepo: document.querySelector("#btnAddRepo"),
        btnSave: document.querySelector("#saveConfig"),
        addRepo: function (config) {
            const template = Artwork.template.cloneNode(true).content;

            template.querySelector("[data-id=txtRepositoryName]").value = config.Name || "";
            template.querySelector("[data-id=txtRepositoryUrl]").value = config.Url || "";

            Artwork.configurationWrapper.appendChild(template);
        },
        saveConfig: function (e) {
            e.preventDefault();
            Dashboard.showLoadingMsg();

            const config = {
                ArtworkRepos: []
            };

            const configs = document.querySelectorAll("[data-id=repo-config]");
            for (let i = 0; i < configs.length; i++) {
                const repo = {
                    Name: configs[i].querySelector("[data-id=txtRepositoryName]").value,
                    Url: configs[i].querySelector("[data-id=txtRepositoryUrl]").value
                };

                config.ArtworkRepos.push(repo);
            }

            window.ApiClient.updatePluginConfiguration(Artwork.pluginId, config)
                .then(Dashboard.processPluginConfigurationUpdateResult)
                .catch(function (error) {
                    console.error(error);
                })
                .finally(function () {
                    Dashboard.hideLoadingMsg();
                });
        },
        loadConfig: function () {
            Dashboard.showLoadingMsg();
            window.ApiClient.getPluginConfiguration(Artwork.pluginId)
                .then(function (config) {
                    for (let i = 0; i < config.ArtworkRepos.length; i++) {
                        Artwork.addRepo(config.ArtworkRepos[i]);
                    }
                })
                .catch(function (error) {
                    console.error(error);
                })
                .finally(function () {
                    Dashboard.hideLoadingMsg();
                });
        },
        init: function () {
            Artwork.btnAddRepo.addEventListener("click", Artwork.addRepo);
            Artwork.btnSave.addEventListener("click", Artwork.saveConfig);
            Artwork.loadConfig();
        }
    }

    view.addEventListener("viewshow", function (e) {
        Artwork.init();
    });
}