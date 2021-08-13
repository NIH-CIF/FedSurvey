import Cookies from 'universal-cookie';

const fetch = (url, obj) => (
    window.fetch(url, { ...obj, headers: { ...(obj?.headers || {}), token: (new Cookies()).get('token') } })
        .then(resp => {
            if (resp.status == 401) {
                alert('You must be logged in to perform this action.');
                window.location.replace('/login');
            }

            return resp;
        })
);

export default {
    fetch
};