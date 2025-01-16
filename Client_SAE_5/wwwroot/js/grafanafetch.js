window.checkGrafanaServer = async function (url) {
    try {
        let response = await fetch(url, { method: 'GET', mode: 'no-cors' });
        return response.ok;
    } catch (error) {
        console.error("Erreur de connexion au serveur", error);
        return false;
    }
};
