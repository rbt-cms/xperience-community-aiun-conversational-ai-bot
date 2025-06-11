import React, { useState, useEffect } from "react";

import { TextArea } from "@kentico/xperience-admin-components";

import { usePageCommand } from "@kentico/xperience-admin-base";


// Props passed from the backend (template properties)


interface SettingsKeyProperties {

    keyValue: string;

}

// Expected shape of the response from the backend PageCommand

 interface SaveResponse {

    message: string;

}

 const Commands = {

    SaveKeyValue: "SaveKeyValue", // Must match [PageCommand] method name

};

export const SettingsKeyLayoutTemplate = ({ keyValue }: SettingsKeyProperties) => {

    const [value, setValue] = useState(keyValue);

    const [message, setMessage] = useState("");

    const [error, setError] = useState("");

    // ✅ Correct generic order: <Response, Input>

    const { execute: submit } = usePageCommand<SaveResponse>(

        Commands.SaveKeyValue,
        {
            after: (res) => {
                if (res?.message) {
                    setMessage(res.message);
                    setError("");
                }
            }

        }

    );

    useEffect(() => {

        setValue(keyValue); // Populate the initial value

    }, [keyValue]);

    //const handleSave = () => {

    //    save({ keyValue: value }); // Pass input to backend

    //};

    const handleSave = () => {
        if (!value.trim()) {
            setError("The 'API Key' field is required.");
            setMessage(""); // Clear success message if any
            return;
        }

        submit(value); // Pass input to backend
    };

    return (
        <div className="api-key-wrapper">
            <div className="size-m___exMBL api-key-text">API Key</div>
            <div className="api-key-text-wrapper">
            <TextArea 
                value={value}
               onChange={(e) => setValue(e.target.value)}
                placeholder="Enter settings key"
            />
            {error && <div style={{ color: "red", margin: "10px" }}>
                <div className="validation-message___WZhc7">
                    <div className='alert-icon___B6642' style={{ display: "flex" }}>
                        <svg width="1em" height="1em" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" role="img" style={{ display: 'block' }} >
                            <path fill-rule="evenodd" clip-rule="evenodd" d="M8 1c.523 0 .798.595.894.789l6.5 12.764A1 1 0 0 1 14.5 16h-13a1 1 0 0 1-.894-1.447l6.5-12.764C7.351 1.295 7.475 1 8 1Zm0 10.5a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3ZM9.5 6h-3l.5 4h2l.5-4Z" fill="currentColor"></path>
                        </svg> {error}
                    </div>
                </div>
            </div>}

            <a aria-label="Save" onClick={handleSave} className="anchor-button___YMii0 button___Ky_Bj type-primary___KNJNo size-m___hQv_w" style={{ marginTop: "10px" }}  > Save</a>

            {message && <div style={{ color: "green", marginTop: "10px" }}>{message}</div>}
            </div>
        </div>

    );

};


