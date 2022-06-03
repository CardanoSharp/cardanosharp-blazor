export function createWalletConnector() {
    return {
        connectedWallet: null,

        isWalletInstalled: function (walletkey) {
            try {
                return (Object.keys(window.cardano).indexOf(walletkey) > -1);
            } catch (e) {
                return false;
            }
        },

        isWalletEnabled: async function (walletkey) {
            return await window.cardano[walletkey].isEnabled();
        },

        getWalletApiVersion: function (walletkey) {
            return window.cardano[walletkey].apiVersion;
        },

        getWalletName: function (walletkey) {
            return window.cardano[walletkey].name;
        },

        getWalletIcon: function (walletkey) {
            return window.cardano[walletkey].icon;
        },

        connectWallet: async function (walletkey) {
            try {
                this.connectedWallet = await window.cardano[walletkey].enable();
                return this.connectedWallet !== null;
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        disconnect: function () {
            this.connectedWallet = null;
        },

        getNetworkId: async function () {
            try {
                return await this.connectedWallet.getNetworkId();
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        getUtxos: async function (amountCbor = undefined, paginate = undefined) {
            try {
                return await this.connectedWallet.getUtxos(amountCbor, paginate);
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        getCollateral: async function (params) {
            try {
                return await this.connectedWallet.getCollateral(params);
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        getBalance: async function () {
            try {
                return await this.connectedWallet.getBalance();
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        getUsedAddresses: async function (paginate = undefined) {
            try {
                return await this.connectedWallet.getUsedAddresses(paginate);
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        getUnusedAddresses: async function () {
            try {
                return await this.connectedWallet.getUnusedAddresses();
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        getChangeAddress: async function () {
            try {
                return await this.connectedWallet.getChangeAddress();
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        getRewardAddresses: async function () {
            try {
                return await this.connectedWallet.getRewardAddresses();
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        signTx: async function (tx, partialSign = false) {
            try {
                return await this.connectedWallet.signTx(tx, partialSign);
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        signData: async function (addr, payload) {
            try {
                return await this.connectedWallet.signData(addr, payload);
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },

        submitTx: async function (tx) {
            try {
                return await this.connectedWallet.submitTx(tx);
            }
            catch (e) {
                throw JSON.stringify(e);
            }
        },
    }
}