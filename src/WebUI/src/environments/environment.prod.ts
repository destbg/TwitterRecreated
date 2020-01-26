export const environment = {
  production: true,
  API_URL:
    typeof window !== 'undefined'
      ? `https://api-${window.location.hostname}/api/`
      : '',
};
