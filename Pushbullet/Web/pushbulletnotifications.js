const PushbulletPluginConfig = {
    uniquePluginId: 'DE228F12-E43E-4BD9-9fC0-2830819C3B92'
};

function loadUserConfig(page, userId) {
    Dashboard.showLoadingMsg();
    ApiClient.getPluginConfiguration(PushbulletPluginConfig.uniquePluginId).then(function (config) {
        const PushbulletConfig = config.Options.filter(function (c) {
            return userId === c.UserId;
        })[0] || {};
        page.querySelector('#chkEnablePushbullet').checked = PushbulletConfig.Enabled || false;
        page.querySelector('#txtPushbulletChannel').value = PushbulletConfig.Channel || '';
        page.querySelector('#txtPushbulletAuthKey').value = PushbulletConfig.Token || '';
        page.querySelector('#txtPushbulletDeviceId').value = PushbulletConfig.DeviceId || '';

        Dashboard.hideLoadingMsg();
    });
}

export default function (view) {
    view.querySelector('#selectUser').addEventListener('change', function () {
        loadUserConfig(view, this.value);
    });

    view.querySelector('#testNotification').addEventListener('click', function () {
        Dashboard.showLoadingMsg();
        const onError = function () {
            Dashboard.alert('There was an error sending the test notification. Please check your notification settings and try again.');
            Dashboard.hideLoadingMsg();
        };

        ApiClient.getPluginConfiguration(PushbulletPluginConfig.uniquePluginId).then(function (config) {
            if (!config.Options.length) {
                Dashboard.hideLoadingMsg();
                Dashboard.alert('Please configure and save at least one notification account.');
            }

            config.Options.map(function (c) {
                ApiClient.ajax({
                    type: 'POST',
                    url: ApiClient.getUrl('Notification/Pushbullet/Test/' + c.UserId)
                }).then(function () {
                    Dashboard.hideLoadingMsg();
                }, onError);
            });
        });
    });

    view.addEventListener('viewshow', function () {
        Dashboard.showLoadingMsg();
        const page = this;
        ApiClient.getUsers().then(function (users) {
            page.querySelector('#selectUser').innerHTML = users.map(function (user) {
                return '<option value="' + user.Id + '">' + user.Name + '</option>';
            });
        });

        Dashboard.hideLoadingMsg();
    });

    view.querySelector('.PushbulletConfigurationForm').addEventListener('submit', function (e) {
        Dashboard.showLoadingMsg();
        const form = this;
        ApiClient.getPluginConfiguration(PushbulletPluginConfig.uniquePluginId).then(function (config) {
            const userId = form.querySelector('#selectUser').value;
            let PushbulletConfig = config.Options.filter(function (c) {
                return userId === c.UserId;
            })[0];

            if (!PushbulletConfig) {
                PushbulletConfig = {};
                config.Options.push(PushbulletConfig);
            }

            PushbulletConfig.UserId = userId;
            PushbulletConfig.Enabled = form.querySelector('#chkEnablePushbullet').checked;
            PushbulletConfig.Channel = form.querySelector('#txtPushbulletChannel').value;
            PushbulletConfig.Token = form.querySelector('#txtPushbulletAuthKey').value;
            PushbulletConfig.DeviceId = form.querySelector('#txtPushbulletDeviceId').value;

            ApiClient.updatePluginConfiguration(PushbulletPluginConfig.uniquePluginId, config).then(function (result) {
                Dashboard.processPluginConfigurationUpdateResult(result);
            });
        });
        e.preventDefault();
        return false;
    });
}
