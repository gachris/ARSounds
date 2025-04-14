export const environment = {
  production: true,
  application_name: 'ARSounds',
  application_version: 'v1',
  authority: 'https://auth.openvision.com',
  api_base_uri: 'https://arsounds.openvision.com',
  application_base_uri: 'https://arsounds.openvision.com',
  redirect_uri: 'https://arsounds.openvision.com/signin-callback-oidc',
  silent_redirect_uri: 'https://arsounds.openvision.com/silent-callback-oidc',
  post_logout_redirect_uri: 'https://arsounds.openvision.com/signout-callback-oidc',
  response_type: "code",
  scope: 'openid profile email roles offline_access arsounds.read arsounds.write',
  client_id: '16c8fb9b-daec-4ffb-a91c-b78eec0ef07c'
};
