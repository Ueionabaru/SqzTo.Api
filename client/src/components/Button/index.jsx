import React from 'react';

import "./button.scss"


const Button = props => {
    return (
        <button 
            {...props}
        className = "btn-sbmt"/>
    );
}

export default Button;