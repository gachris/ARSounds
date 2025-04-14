export const environment = {
  production: false,
  application_name: 'ARSounds',
  application_version: 'v1',
  authority: 'https://localhost:44310',
  api_base_uri: 'https://localhost:7266',
  application_base_uri: 'https://localhost:44439',
  redirect_uri: 'https://localhost:44439/signin-callback-oidc',
  silent_redirect_uri: 'https://localhost:44439/silent-callback-oidc',
  post_logout_redirect_uri: 'https://localhost:44439/signout-callback-oidc',
  response_type: "code",
  scope: 'openid profile email roles offline_access arsounds.read arsounds.write',
  client_id: '16c8fb9b-daec-4ffb-a91c-b78eec0ef07c'
};
