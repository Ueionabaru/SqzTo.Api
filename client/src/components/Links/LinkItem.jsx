import React, { useRef, useState } from 'react';
import {Input, Button} from 'components'
import './links.scss'

const LinkItem = ({url, longUrl}) => {
    const [copySuccess, setCopySuccess] = useState('КОПИРОВАТЬ');
    //const linkRef = useRef(null);

    function  copyFunc (e) {
        //linkRef.current.select();
        //document.execCommand("copy");
        //e.target.focus();
        setCopySuccess('СКОПИРОВАНО!');
    }
    return (
        <div className = "linkitem-content">
            <span className = 'linkitem-content-originUrl'>
                {longUrl}
            </span>
            <a href = {url} className = 'linkitem-content-sqzLink'>
                {url}
            </a>
            <Button onClick = {copyFunc}>
                {copySuccess}
            </Button>
        </div>
    );
}

export default LinkItem;