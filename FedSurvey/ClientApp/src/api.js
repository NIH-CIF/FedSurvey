import Cookies from 'universal-cookie';

const fetch = (url, obj) => (
    window.fetch(url, { ...obj, headers: { ...(obj?.headers || {}), token: (new Cookies()).get('token')}})
);

export default {
    fetch
};