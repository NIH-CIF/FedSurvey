import Cookies from 'universal-cookie';

// when developing, maybe worth setting this to false
const LOGIN_ENABLED = true;

const hasToken = () => (
    (new Cookies()).get('token')
);

export {
    LOGIN_ENABLED,
    hasToken
};