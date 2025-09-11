import type { UserManagerSettings } from 'oidc-client-ts';

export const authSettings: UserManagerSettings = {
    // The URL of your IdentityServer. Make sure the port matches
    // the one your Identity.Oli.Auth project is running on.
    // You can find this in Properties/launchSettings.json.
    // It's often 7001 for HTTPS in a default VS template.
    authority: 'https://localhost:7040',

    // The client ID you configured in Config.cs
    client_id: 'spa-client',

    // The URL to redirect to after the user logs in
    redirect_uri: 'http://localhost:5173/callback',

    // The URL to redirect to after the user logs out
    post_logout_redirect_uri: 'http://localhost:5173/',

    // Using the Authorization Code Flow with PKCE
    response_type: 'code',

    // The scopes (permissions) your client is requesting
    scope: 'openid profile api1.read',
};